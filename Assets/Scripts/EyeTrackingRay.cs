using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input;
using Unity.VisualScripting;
using UnityEngine;

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
    private Color rayColorDefautState = Color.yellow;

    [SerializeField]
    private Color rayColorHoverState = Color.red;

    [SerializeField]
    private OVRHand handUsedForPinchSelection;

    [SerializeField]
    private bool mockHandUsedForPinchSelection;

    private bool intercepting;

    private bool allowPinchSelection;

    private LineRenderer lineRenderer;

    private Dictionary<int, EyeInteractable> interactables = new Dictionary<int, EyeInteractable>();

    private EyeInteractable lastEyeInteractable;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found!");
            return;
        }
        layersToInclude = LayerMask.GetMask("EyeInteractable");
        allowPinchSelection = handUsedForPinchSelection != null;
        SetupRay();
    }

    void SetupRay()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
        lineRenderer.startColor = rayColorDefautState;
        lineRenderer.endColor = rayColorDefautState;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, transform.position.z + rayDistance));
    }

    private void Update()
    {
        lineRenderer.enabled = !IsPinching();

        SelectionStarted();

        // Clear all hover states
        if (!intercepting)
        {
            lineRenderer.startColor = lineRenderer.endColor = rayColorDefautState;
            lineRenderer.SetPosition(1, new Vector3(0, 0, transform.position.z + rayDistance));
            OnHoverEnded();
        }
    }

    private void SelectionStarted()
    {
        if (intercepting && IsPinching())
        {
            lastEyeInteractable?.Select(true, (handUsedForPinchSelection?.IsTracked ?? false) ? handUsedForPinchSelection.transform : transform);
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
    }

    private void OnDestroy()
    {
        interactables.Clear();
    }

    private bool IsPinching()
    {
        return (allowPinchSelection && handUsedForPinchSelection != null && handUsedForPinchSelection.GetFingerIsPinching(OVRHand.HandFinger.Index)) || mockHandUsedForPinchSelection;
    }
}
