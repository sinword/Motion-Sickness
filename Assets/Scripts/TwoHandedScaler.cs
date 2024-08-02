using Oculus.Interaction.Input;
using UnityEngine;

public class TwoHandedScaler : MonoBehaviour
{
    public Hand leftHandComponent;
    public Hand rightHandComponent;
    public Transform leftHandPosition;
    public Transform rightHandPosition;

    private Vector3 initialScale;
    private float initialDistance;
    private bool isScaling;

    void Start()
    {
        initialScale = transform.localScale;
        isScaling = false;
    }

    void Update()
    {
        bool leftHandPinching = leftHandComponent.GetFingerIsPinching(HandFinger.Index);
        bool rightHandPinching = rightHandComponent.GetFingerIsPinching(HandFinger.Index);

        if (leftHandPinching && rightHandPinching)
        {
            if (!isScaling)
            {
                // Initialize scaling
                initialDistance = Vector3.Distance(leftHandPosition.position, rightHandPosition.position);
                initialScale = transform.localScale;
                isScaling = true;
            }

            // Perform scaling
            ChangeScale();
        }
        else
        {
            // Reset scaling flag
            isScaling = false;
        }
    }

    private void ChangeScale()
    {
        float currentDistance = Vector3.Distance(leftHandPosition.position, rightHandPosition.position);
        float scaleFactor = currentDistance / initialDistance;
        transform.localScale = initialScale * scaleFactor;
    }
}
