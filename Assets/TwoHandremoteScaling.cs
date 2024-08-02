using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input;
using UnityEngine;

public class TwoHandremoteScaling : MonoBehaviour
{
    private bool isScaling = false;
    private Vector3 initialScale;
    private float initialDistanceBetweenHands;
    [SerializeField]
    private Hand leftHandComponent;
    [SerializeField]
    private Hand rightHandComponent;
    [SerializeField]
    private Transform leftHandTransform;
    [SerializeField]
    private Transform rightHandTransform;

    public Transform LeftHandTransform
    {
        get { return leftHandTransform; }
        set { leftHandTransform = value; }
    }

    public Transform RightHandTransform
    {
        get { return rightHandTransform; }
        set { rightHandTransform = value; }
    }

    void Update()
    {
        if (isScaling && leftHandTransform != null && rightHandTransform != null)
        {
            float currentDistanceBetweenHands = Vector3.Distance(leftHandTransform.position, rightHandTransform.position);
            float scaleFactor = currentDistanceBetweenHands / initialDistanceBetweenHands;
            transform.localScale = initialScale * scaleFactor;
        }
        else
        {
            UpdateHandState(leftHandComponent, ref isScaling, true);
            UpdateHandState(rightHandComponent, ref isScaling, false);
        }
    }

    private void UpdateHandState(Hand handComponent, ref bool isHandGrabbing, bool isLeftHand)
    {
        if (handComponent.GetFingerIsPinching(HandFinger.Index))
        {
            if (!isHandGrabbing)
            {
                isHandGrabbing = true;
                if (isLeftHand)
                {
                    LeftHandTransform = handComponent.transform;
                }
                else
                {
                    RightHandTransform = handComponent.transform;
                }

                // Ensure both hands are detected before calling OnTwoHandGrab
                if (LeftHandTransform != null && RightHandTransform != null)
                {
                    OnTwoHandGrab(LeftHandTransform, RightHandTransform);
                }
            }
        }
        else
        {
            if (isHandGrabbing)
            {
                isHandGrabbing = false;
                if (isLeftHand)
                {
                    LeftHandTransform = null;
                }
                else
                {
                    RightHandTransform = null;
                }

                // If either hand is released, stop scaling
                if (LeftHandTransform == null || RightHandTransform == null)
                {
                    OnTwoHandRelease();
                }
            }
        }
    }

    public void OnTwoHandGrab(Transform leftHand, Transform rightHand)
    {
        initialDistanceBetweenHands = Vector3.Distance(leftHand.position, rightHand.position);
        Debug.LogWarning("Initial distance between hands: " + initialDistanceBetweenHands);
        if (initialDistanceBetweenHands > 0)
        {
            initialScale = transform.localScale;
            isScaling = true;
        }
        else
        {
            Debug.LogWarning("Initial distance between hands is zero. Scaling will not be performed.");
            isScaling = false;
        }
    }

    public void OnTwoHandRelease()
    {
        isScaling = false;
    }
}
