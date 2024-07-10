using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionFollower : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float distance = 3.0f;
    [SerializeField]
    private float shiftSpeed = 0.025f;

    private bool isCentered = false;
    private Renderer[] childRenderers;

    void Start()
    {
        childRenderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        // Check if all child renderers are not visible
        if (!AreAnyChildRenderersVisible())
        {
            isCentered = false;
            Vector3 targetPosition = FindTargetPosition();
            MoveTowards(targetPosition);
            
            if (ReachedPosition(targetPosition))
            {
                isCentered = true;
            }
        }
    }

    private bool AreAnyChildRenderersVisible()
    {
        foreach (Renderer renderer in childRenderers)
        {
            Debug.Log(renderer.name + " is visible: " + renderer.isVisible);
            if (renderer.isVisible)
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 FindTargetPosition()
    {
        return cameraTransform.position + (cameraTransform.forward * distance);
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        transform.position += (targetPosition - transform.position) * shiftSpeed;
        transform.LookAt(cameraTransform);
        transform.Rotate(0, 180, 0);
    }

    private bool ReachedPosition(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, transform.position) < 0.1f;
    }
}
