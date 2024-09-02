using System.Collections.Generic;
using System.Reflection;
using Oculus.Interaction.Input;
using UnityEngine;
using Oculus.Interaction.DistanceReticles;

[RequireComponent(typeof(LineRenderer))]

public class EyeTrackingRay : MonoBehaviour
{
    [SerializeField]
    private float rayDistance = 1.0f;

    [SerializeField]
    private float rayWidth = 0.01f;

    [SerializeField]
    private LayerMask layersToInclude;

    [SerializeField]
    private Color rayColorDefaultState = Color.yellow;

    [SerializeField]
    private Color rayColorHoverState = Color.red;

    [SerializeField]
    private OVRHand mainHand;

    [SerializeField]
    private OVRHand secondaryHand;

    [SerializeField]
    private bool mockMainHand;
    [SerializeField]
    private ReticleIconDrawer reticleIconDrawer;
    private bool intercepting;

    private bool allowPinchSelection;
    private bool allowResize;

    private LineRenderer lineRenderer;

    private Dictionary<int, EyeInteractable> interactables = new Dictionary<int, EyeInteractable>();

    private EyeInteractable lastEyeInteractable;
    private float initialPinchDistance;
    private bool isResizing;

    // MethodInfo for the protected methods
    private MethodInfo drawMethod;
    private MethodInfo hideMethod;
    private ReticleDataIcon reticleDataIcon;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found!");
            return;
        }
        layersToInclude = LayerMask.GetMask("EyeInteractable");
        allowPinchSelection = mainHand != null && secondaryHand != null;
        SetupRay();

        // Use reflection to get access to the protected methods
        drawMethod = typeof(ReticleIconDrawer).GetMethod("Draw", BindingFlags.Instance | BindingFlags.NonPublic);
        hideMethod = typeof(ReticleIconDrawer).GetMethod("Hide", BindingFlags.Instance | BindingFlags.NonPublic);
        
        if (drawMethod == null || hideMethod == null)
        {
            Debug.LogError("Failed to access the protected methods using reflection.");
        }

        reticleDataIcon = new ReticleDataIcon();
        reticleDataIcon.CustomIcon = reticleIconDrawer.DefaultIcon;

        // Subscribe to selection changed events
        foreach (var interactable in interactables)
        {
            interactable.Value.OnSelectionChanged.AddListener(OnObjectSelected);
        }
    }

    void SetupRay()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
        lineRenderer.startColor = rayColorDefaultState;
        lineRenderer.endColor = rayColorDefaultState;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, new Vector3(0, 0, rayDistance));
    }

    private void Update()
    {
        lineRenderer.enabled = !IsPinching();

        SelectionStarted();

        // Clear all hover states
        if (!intercepting)
        {
            lineRenderer.startColor = lineRenderer.endColor = rayColorDefaultState;
            lineRenderer.SetPosition(1, new Vector3(0, 0, rayDistance));
            OnHoverEnded();
        }

        // if (IsResizing())
        // {
        //     ResizeInteractable();
        // }
        // else {
        //     isResizing = false;
        // }
    }

    private void SelectionStarted()
    {
        if (intercepting && IsPinching())
        {
            Transform pinchingHandTransform = null;

            if (mainHand != null && mainHand.IsTracked && mainHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
            {
                pinchingHandTransform = mainHand.transform;
            }
            else if (secondaryHand != null && secondaryHand.IsTracked && secondaryHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
            {
                pinchingHandTransform = secondaryHand.transform;
            }

            lastEyeInteractable?.Select(true, pinchingHandTransform ?? transform);
        }
        else
        {
            lastEyeInteractable?.Select(false);
        }
    }


    void FixedUpdate()
    {
        if (IsPinching()) return;

        Vector3 rayDirection = transform.TransformDirection(Vector3.forward) * rayDistance;

        intercepting = Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, Mathf.Infinity, layersToInclude);

        if (intercepting)
        {
            Debug.Log("Hit object: " + hit.transform.gameObject.name);
            OnHoverEnded();
            lineRenderer.startColor = lineRenderer.endColor = rayColorHoverState;

            // Ensure the hit object has EyeInteractable component
            var eyeInteractable = hit.transform.GetComponent<EyeInteractable>();
            if (eyeInteractable)
            {
                // Keep cache of eye interactable
                if (!interactables.TryGetValue(hit.transform.gameObject.GetHashCode(), out EyeInteractable cachedInteractable))
                {
                    interactables.Add(hit.transform.gameObject.GetHashCode(), eyeInteractable);
                }

                var toLocalSpace = transform.InverseTransformPoint(eyeInteractable.transform.position);
                lineRenderer.SetPosition(1, new Vector3(0, 0, toLocalSpace.z));

                eyeInteractable.Hover(true);

                lastEyeInteractable = eyeInteractable;

                // Use reflection to invoke the Draw method
                if (drawMethod != null)
                {
                    // Set reticle position to the 3D center of the object
                    reticleIconDrawer.transform.position = eyeInteractable.GetComponent<Collider>().bounds.center;
                    reticleIconDrawer.transform.forward = -rayDirection.normalized;
                    drawMethod.Invoke(reticleIconDrawer, new object[] { reticleDataIcon });
                }
            }
            else
            {
                // Log detailed error if EyeInteractable component not found
                Debug.LogError("EyeInteractable component not found on hit object: " + hit.transform.gameObject.name
                    + ". Transform: " + hit.transform.name + ", Layer: " + hit.transform.gameObject.layer);
                return;
            }
        }
        else
        {
            Debug.Log("No object hit by Raycast");
            if (hideMethod != null)
            {
                hideMethod.Invoke(reticleIconDrawer, null); // Use reflection to invoke the Hide method
            }
        }
    }

    private void OnHoverEnded()
    {
        foreach (var interactable in interactables)
        {
            if (interactable.Value)
            {
                interactable.Value.Hover(false);
            }
            else
            {
                Debug.LogWarning("Null EyeInteractable in interactable dictionary!");
            }
        }
        
        // Hide the reticle when no object is hovered
        if (hideMethod != null)
        {
            hideMethod.Invoke(reticleIconDrawer, null);
        }
    }

    // Method to handle selection change
    public void OnObjectSelected(GameObject obj)
    {
        if (obj.TryGetComponent(out EyeInteractable interactable))
        {
            if (interactable.IsSelected && hideMethod != null)
            {
                // Hide reticle when object is selected
                hideMethod.Invoke(reticleIconDrawer, null); 
            }
        }
    }

    private void OnDestroy()
    {
        interactables.Clear();
    }
    

    private bool IsPinching()
    {
        bool mainHandIsPinching = mainHand != null && mainHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        bool secondaryHandIsPinching = secondaryHand != null && secondaryHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        return (allowPinchSelection && (mainHandIsPinching || secondaryHandIsPinching)) || mockMainHand;
    }

    private bool IsResizing()
    {
        // Use two hands pinch to resize
        bool mainHandIsPinching = mainHand != null && mainHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        bool secondaryHandIsPinching = secondaryHand != null && secondaryHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        bool currentlyResizing = allowPinchSelection && mainHandIsPinching && secondaryHandIsPinching;

        if (currentlyResizing && !isResizing)
        {
            // Initalize resizing
            initialPinchDistance = Vector3.Distance(mainHand.transform.position, secondaryHand.transform.position);
            lastEyeInteractable?.SetBaseScale();
            isResizing = true;
        }

        return currentlyResizing;
    }

    private void ResizeInteractable()
    {
        if (lastEyeInteractable && isResizing)
        {
            Vector3 mainHandPosition = mainHand.transform.position;
            Vector3 secondaryHandPosition = secondaryHand.transform.position;
            float currentDistance = Vector3.Distance(mainHandPosition, secondaryHandPosition);
            float ratio = currentDistance / initialPinchDistance;
            lastEyeInteractable.Resize(ratio);
        }
    }
}
