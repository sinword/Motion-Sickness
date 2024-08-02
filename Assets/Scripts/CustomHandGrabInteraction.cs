using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;

public class CustomHandGrabInteraction : MonoBehaviour
{
    [SerializeField]
    private TwoHandremoteScaling twoHandScaling;

    private Hand leftHandComponent;
    private Hand rightHandComponent;

    private void OnEnable()
    {
        // Ensure the Hand components are assigned
        leftHandComponent = GameObject.Find("LeftHand").GetComponent<Hand>();
        rightHandComponent = GameObject.Find("RightHand").GetComponent<Hand>();

        if (leftHandComponent != null)
        {
            leftHandComponent.WhenHandUpdated += OnLeftHandUpdated;
        }

        if (rightHandComponent != null)
        {
            rightHandComponent.WhenHandUpdated += OnRightHandUpdated;
        }
    }

    private void OnDisable()
    {
        if (leftHandComponent != null)
        {
            leftHandComponent.WhenHandUpdated -= OnLeftHandUpdated;
        }

        if (rightHandComponent != null)
        {
            rightHandComponent.WhenHandUpdated -= OnRightHandUpdated;
        }
    }

    private void OnLeftHandUpdated()
    {
        UpdateHandState(leftHandComponent, true);
    }

    private void OnRightHandUpdated()
    {
        UpdateHandState(rightHandComponent, false);
    }

    private void UpdateHandState(Hand handComponent, bool isLeftHand)
    {
        if (handComponent.GetFingerIsPinching(HandFinger.Index))
        {
            if (isLeftHand)
            {
                twoHandScaling.LeftHandTransform = handComponent.transform;
            }
            else
            {
                twoHandScaling.RightHandTransform = handComponent.transform;
            }

            // Ensure both hands are detected before calling OnTwoHandGrab
            if (twoHandScaling.LeftHandTransform != null && twoHandScaling.RightHandTransform != null)
            {
                twoHandScaling.OnTwoHandGrab(twoHandScaling.LeftHandTransform, twoHandScaling.RightHandTransform);
            }
        }
        else
        {
            if (isLeftHand)
            {
                twoHandScaling.LeftHandTransform = null;
            }
            else
            {
                twoHandScaling.RightHandTransform = null;
            }

            // If either hand is released, stop scaling
            if (twoHandScaling.LeftHandTransform == null || twoHandScaling.RightHandTransform == null)
            {
                twoHandScaling.OnTwoHandRelease();
            }
        }
    }
}
