using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PanelVisionFollower : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float distance = 2.5f;
    [SerializeField]
    private float shiftSpeed = 0.015f;
    [SerializeField]
    private float positionThreshold = 0.2f;
    private bool isCentered = true;
    private CanvasRenderer panelCanvasRenderer;
    private RectTransform panelRectTransform;

    void Start()
    {
        // Only find the CanvasRenderer under "Panel" object
        panelCanvasRenderer = transform.Find("SpotTheDiff Canvas/Panel").GetComponent<CanvasRenderer>();
        if (panelCanvasRenderer == null)
        {
            Debug.LogError("Panel CanvasRenderer not found.");
        }
        panelRectTransform = panelCanvasRenderer.GetComponent<RectTransform>();
        // childCanvasRenderers = GetComponentsInChildren<CanvasRenderer>();
        // foreach (CanvasRenderer canvasRenderer in childCanvasRenderers)
        // {
        //     Debug.LogWarning(canvasRenderer.gameObject.name);
        // }
    }

    void Update()
    {
        if (!AreAnyChildCanvasRenderersVisible() && !isCentered)
        {
            isCentered = false;
            Vector3 targetPosition = FindTargetPosition();
            MoveTowards(targetPosition);

            if (ReachedPosition(targetPosition))
            {
                Debug.LogWarning("Reached target position");
                isCentered = true;
            }
        }
    }

    private bool AreAnyChildCanvasRenderersVisible()
    {
        // foreach (CanvasRenderer canvasRenderer in childCanvasRenderers)
        // {
        //     RectTransform rectTransform = canvasRenderer.GetComponent<RectTransform>();
        //     if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, rectTransform.position, Camera.main))
        //     {
        //         Debug.LogWarning(canvasRenderer.gameObject.name + " is visible.");
        //         return true;
        //     }
        // }
        // isCentered = false;
        // return false;
        if (RectTransformUtility.RectangleContainsScreenPoint(panelRectTransform, panelRectTransform.position, Camera.main))
        {
            Debug.LogWarning("Panel is visible.");
            return true;
        }
        else 
        {
            isCentered = false;
            return false;
        }
    }

    private Vector3 FindTargetPosition()
    {
        return cameraTransform.position + (cameraTransform.forward * distance);
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        // During the transition, the distance between the camera and the object keeps does not change
        
        transform.position += (targetPosition - transform.position) * shiftSpeed;
        transform.LookAt(cameraTransform);
        transform.Rotate(0, 180, 0);
        // transform.position = Vector3.MoveTowards(transform.position, targetPosition, shiftSpeed);
        // transform.LookAt(cameraTransform);
        // transform.Rotate(0, 180, 0);
    }

    private bool ReachedPosition(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, transform.position) < positionThreshold;
    }
}
