using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVisionFollower : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float distance = 2.5f;
    [SerializeField]
    private float shiftSpeed = 0.015f;
    [SerializeField]
    private float positionThreshold = 0.2f;
    private bool isCentered = false;

    // Built-in Unity method
    private void OnBecameInvisible()
    {
        Debug.LogWarning("Object became invisible");
        isCentered = false;
    }

    void Update()
    {
        if (!isCentered)
        {
            Vector3 targetPosition = FindTargetPosition();
            MoveTowards(targetPosition);

            if (ReachedPosition(targetPosition))
            {
                isCentered = true;
            }
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
    }

    private bool ReachedPosition(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, transform.position) < positionThreshold;
    }
}
