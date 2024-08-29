using System.Collections.Generic;
using UnityEngine;
using System;

namespace Oculus.Interaction
{
    public class CustomTwoGrabFreeTransformer : MonoBehaviour, ITransformer
    {
        private Quaternion _activeRotation;
        private Vector3 _initialLocalScale;
        private float _initialDistance;
        private float _initialScale = 1.0f;
        private float _activeScale = 1.0f;

        private Pose _previousGrabPointA;
        private Pose _previousGrabPointB;

        [Serializable]
        public class TwoGrabFreeConstraints
        {
            public bool ConstraintsAreRelative;
            public FloatConstraint MinScale;
            public FloatConstraint MaxScale;

            public bool ConstrainXScale = true;
            public bool ConstrainYScale = false;
            public bool ConstrainZScale = false;

            public bool ConstrainXRotation = false; // Constraint for X rotation
            public bool ConstrainYRotation = false; // Constraint for Y rotation
            public bool ConstrainZRotation = false; // Constraint for Z rotation

            public bool ConstrainXPosition = false; // Constraint for X position
            public bool ConstrainYPosition = false; // Constraint for Y position
            public bool ConstrainZPosition = false; // Constraint for Z position

            public Vector2 XPositionLimits = new Vector2(-1f, 1f); // Min and Max local X position
            public Vector2 YPositionLimits = new Vector2(-1f, 1f); // Min and Max local Y position
            public Vector2 ZPositionLimits = new Vector2(-1f, 1f); // Min and Max local Z position
        }

        [SerializeField]
        private TwoGrabFreeConstraints _constraints;

        public TwoGrabFreeConstraints Constraints
        {
            get { return _constraints; }
            set { _constraints = value; }
        }

        private IGrabbable _grabbable;

        public void Initialize(IGrabbable grabbable)
        {
            _grabbable = grabbable;
        }

        public void BeginTransform()
        {
            var grabA = _grabbable.GrabPoints[0];
            var grabB = _grabbable.GrabPoints[1];

            Vector3 diff = grabB.position - grabA.position;
            _activeRotation = Quaternion.LookRotation(diff, Vector3.up).normalized;
            _initialDistance = diff.magnitude;

            if (!_constraints.ConstraintsAreRelative)
            {
                _activeScale = _grabbable.Transform.localScale.x;
            }
            _initialScale = _activeScale;
            _initialLocalScale = _grabbable.Transform.localScale / _initialScale;

            _previousGrabPointA = new Pose(grabA.position, grabA.rotation);
            _previousGrabPointB = new Pose(grabB.position, grabB.rotation);
        }

        public void UpdateTransform()
        {
            var grabA = _grabbable.GrabPoints[0];
            var grabB = _grabbable.GrabPoints[1];
            var targetTransform = _grabbable.Transform;

            Vector3 initialCenter = Vector3.Lerp(_previousGrabPointA.position, _previousGrabPointB.position, 0.5f);
            Vector3 targetCenter = Vector3.Lerp(grabA.position, grabB.position, 0.5f);

            Quaternion initialRotation = _activeRotation;
            Vector3 initialVector = _previousGrabPointB.position - _previousGrabPointA.position;
            Vector3 targetVector = grabB.position - grabA.position;
            Quaternion baseRotation = Quaternion.FromToRotation(initialVector, targetVector);

            Quaternion deltaA = grabA.rotation * Quaternion.Inverse(_previousGrabPointA.rotation);
            Quaternion halfDeltaA = Quaternion.Slerp(Quaternion.identity, deltaA, 0.5f);

            Quaternion deltaB = grabB.rotation * Quaternion.Inverse(_previousGrabPointB.rotation);
            Quaternion halfDeltaB = Quaternion.Slerp(Quaternion.identity, deltaB, 0.5f);

            Quaternion baseTargetRotation = baseRotation * halfDeltaA * halfDeltaB * initialRotation;

            Vector3 upDirection = baseTargetRotation * Vector3.up;
            Quaternion targetRotation = Quaternion.LookRotation(targetVector, upDirection).normalized;

            // Apply rotation constraints
            Vector3 eulerRotation = targetRotation.eulerAngles;
            if (_constraints.ConstrainXRotation) eulerRotation.x = 0;
            if (_constraints.ConstrainYRotation) eulerRotation.y = 0;
            if (_constraints.ConstrainZRotation) eulerRotation.z = 0;

            targetRotation = Quaternion.Euler(eulerRotation);

            _activeRotation = targetRotation;

            float activeDistance = targetVector.magnitude;
            if (Mathf.Abs(activeDistance) < 0.0001f) activeDistance = 0.0001f;

            float scalePercentage = activeDistance / _initialDistance;

            float previousScale = _activeScale;
            _activeScale = _initialScale * scalePercentage;

            var nextScale = _activeScale * _initialLocalScale;

            if (_constraints.MinScale.Constrain)
            {
                float scalar = 1f;
                if (_constraints.ConstrainXScale)
                {
                    scalar = Mathf.Max(scalar, _constraints.MinScale.Value / nextScale.x);
                }
                if (_constraints.ConstrainYScale)
                {
                    scalar = Mathf.Max(scalar, _constraints.MinScale.Value / nextScale.y);
                }
                if (_constraints.ConstrainZScale)
                {
                    scalar = Mathf.Max(scalar, _constraints.MinScale.Value / nextScale.z);
                }
                nextScale *= scalar;
            }

            if (_constraints.MaxScale.Constrain)
            {
                float scalar = 1f;
                if (_constraints.ConstrainXScale)
                {
                    scalar = Mathf.Min(scalar, _constraints.MaxScale.Value / nextScale.x);
                }
                if (_constraints.ConstrainYScale)
                {
                    scalar = Mathf.Min(scalar, _constraints.MaxScale.Value / nextScale.y);
                }
                if (_constraints.ConstrainZScale)
                {
                    scalar = Mathf.Min(scalar, _constraints.MaxScale.Value / nextScale.z);
                }
                nextScale *= scalar;
            }

            _activeScale = nextScale.x / _initialLocalScale.x;

            Vector3 worldOffsetFromCenter = targetTransform.position - initialCenter;
            Vector3 offsetInTargetSpace = Quaternion.Inverse(initialRotation) * worldOffsetFromCenter;
            offsetInTargetSpace /= previousScale;

            Quaternion rotationInTargetSpace = Quaternion.Inverse(initialRotation) * targetTransform.rotation;

            // Convert the world position to local space for constraint checks
            Vector3 localTargetPosition = targetTransform.parent.InverseTransformPoint((targetRotation * (_activeScale * offsetInTargetSpace)) + targetCenter);

            // Apply local position constraints
            if (_constraints.ConstrainXPosition)
            {
                localTargetPosition.x = Mathf.Clamp(localTargetPosition.x, _constraints.XPositionLimits.x, _constraints.XPositionLimits.y);
            }
            if (_constraints.ConstrainYPosition)
            {
                localTargetPosition.y = Mathf.Clamp(localTargetPosition.y, _constraints.YPositionLimits.x, _constraints.YPositionLimits.y);
            }
            if (_constraints.ConstrainZPosition)
            {
                localTargetPosition.z = Mathf.Clamp(localTargetPosition.z, _constraints.ZPositionLimits.x, _constraints.ZPositionLimits.y);
            }

            // Convert the constrained local position back to world space
            Vector3 constrainedWorldPosition = targetTransform.parent.TransformPoint(localTargetPosition);

            targetTransform.position = constrainedWorldPosition;
            targetTransform.rotation = targetRotation * rotationInTargetSpace;
            targetTransform.localScale = nextScale;

            _previousGrabPointA = new Pose(grabA.position, grabA.rotation);
            _previousGrabPointB = new Pose(grabB.position, grabB.rotation);
        }

        public void MarkAsBaseScale()
        {
            _activeScale = 1.0f;
        }

        public void EndTransform() { }

        #region Inject

        public void InjectOptionalConstraints(TwoGrabFreeConstraints constraints)
        {
            _constraints = constraints;
        }

        #endregion
    }
}
