using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

// Task 2 only at current stage

namespace DistanceInteractableTask
{
    public enum TaskRadius
    {
        Low,
        Medium,
        High
    }

    public enum TaskDimension
    {
        TwoDimensional,
        ThreeDimensional
    }

    // public enum TaskType
    // {
    //     Level1,
    //     Level2,
    //     Level3
    // }

    // Level1: Select
    // Level2: Drag and drop
    // Level3: Drag and drop, rotate, scale

    public enum State
    {
        Interaction,
        Pause
    }

    public class HandInteractableTaskController : MonoBehaviour
    {
        [SerializeField]
        private GameObject interactionArea;
        [SerializeField]
        private GameObject interactableCube;

        [SerializeField]
        private GameObject targetCube;

        [SerializeField]
        private TextMeshProUGUI timeText;   

        [SerializeField]
        private TaskRadius taskRadius;
        [SerializeField]
        private TaskDimension taskDimension;
        [SerializeField]
        private float minRadius = 0.3f;
        [SerializeField]
        private float maxRadius = 0.6f;
        // [SerializeField]
        // private TaskType taskType;
        [SerializeField]
        private float roundTime = 5f;
        [SerializeField]
        private float pauseTime = 1f;
        private State state = State.Interaction;
        private float timeLeft;
        private string report;
        private Vector3 interactableCubeInitialScale;
        private Vector3 targetCubeInitialScale;
        private float distanceToCenter;
        private float minDistanceBetweenCubes;
        private TwoGrabFreeTransformer twoGrabFreeTransformer;

        void Awake()
        {
            interactableCubeInitialScale = interactableCube.transform.localScale;
            
        }

        void Start()
        {
            timeLeft = roundTime;
            interactableCubeInitialScale = interactableCube.transform.localScale;
            twoGrabFreeTransformer = interactableCube.GetComponent<TwoGrabFreeTransformer>();
            switch (taskDimension)
            {
                case TaskDimension.TwoDimensional:
                    interactableCube.transform.localScale = new Vector3(interactableCube.transform.localScale.x, interactableCube.transform.localScale.y, 0.001f);
                    targetCube.transform.localScale = new Vector3(targetCube.transform.localScale.x, targetCube.transform.localScale.y, 0.001f);
                    switch (taskRadius)
                    {
                        case TaskRadius.Low:
                            // 30 degree FOV
                            distanceToCenter = maxRadius * Mathf.Tan(15 * Mathf.Deg2Rad);
                            break;
                        case TaskRadius.Medium:
                            // 60 degree FOV
                            distanceToCenter = maxRadius * Mathf.Tan(30 * Mathf.Deg2Rad);
                            break;
                        case TaskRadius.High:
                            // 90 degree FOV
                            distanceToCenter = maxRadius * Mathf.Tan(45 * Mathf.Deg2Rad);
                            break;
                    }
                    break;
                case TaskDimension.ThreeDimensional:
                    interactableCube.transform.localScale = new Vector3(interactableCube.transform.localScale.x, interactableCube.transform.localScale.y, interactableCube.transform.localScale.z);
                    targetCube.transform.localScale = new Vector3(targetCube.transform.localScale.x, targetCube.transform.localScale.y, targetCube.transform.localScale.z);
                    break;
            }
            // if (taskType == TaskType.Level1)
            // {
            //     targetCube.SetActive(false);
            // }

            ResetInteractableCube();
        }

        void Update()
        {
            timeLeft -= Time.deltaTime;
            timeText.text = ((int)timeLeft).ToString();

            // switch (taskType)
            // {
            //     case TaskType.Level1:
            //         // Disable two grab free transformer script

            //         if (twoGrabFreeTransformer != null)
            //         {
            //             twoGrabFreeTransformer.enabled = false;
            //         }
            //         break;
            //     case TaskType.Level2:
            //         if (twoGrabFreeTransformer != null)
            //         {
            //             twoGrabFreeTransformer.enabled = false;
            //         }
            //         else
            //         {
            //             Debug.LogWarning("TwoGrabFreeTransformer is null");
            //         }
            //         break;
            //     case TaskType.Level3:
            //         if (twoGrabFreeTransformer != null)
            //         {
            //             twoGrabFreeTransformer.enabled = true;
            //             interactableCube.transform.localRotation = Quaternion.Euler(0, 0, interactableCube.transform.localRotation.eulerAngles.z);
            //         }
            //         break;
            // }
            
            switch (state)
            {
                case State.Interaction:
                    if (timeLeft <= 0)
                    {
                        report = Evaluation();
                        Debug.LogWarning(report);
                        InteractionPause();
                        timeLeft = pauseTime;
                        state = State.Pause;
                    }
                    break;
                case State.Pause:
                    if (timeLeft <= 0)
                    {
                        ResetInteractableCube();
                        timeLeft = roundTime;
                        state = State.Interaction;
                    }
                    break;
            }
            
        }

        void LateUpdate()
        {
            // if (taskType == TaskType.Level3)
            // {
            //     interactableCube.transform.localRotation = Quaternion.Euler(0, 0, interactableCube.transform.localRotation.eulerAngles.z);
            // }
        }

        private void ResetInteractableCube()
        {
            float xInteractablePos = 0;
            float yInteractablePos = 0;
            float zInteractablePos = 0;
            // float zInteractableRotate = 0;
            // float interactableScale = 1;
            interactableCube.SetActive(true);
            targetCube.SetActive(true);

            // switch (taskType)
            // {
            //     case TaskType.Level1:
            //         // Task: Select
            //         xInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
            //         yInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
            //         break;
            //     case TaskType.Level2:
            //         // Task: Drag and drop
            //         xInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
            //         yInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
            //         ResetTargetCube(xInteractablePos, yInteractablePos);
            //         break;
            //     case TaskType.Level3:
            //         // Task: Drag and drop, rotate, scale
            //         xInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
            //         yInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
            //         zInteractableRotate = Random.Range(0, 90);
            //         interactableScale = Random.Range(0.5f, 2f);
            //         ResetTargetCube(xInteractablePos, yInteractablePos);
            //         break;
            // }

            switch (taskDimension)
            {
                case TaskDimension.TwoDimensional:
                    xInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
                    yInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
                    zInteractablePos = maxRadius;
                    minDistanceBetweenCubes = distanceToCenter;
                    ResetTargetCube2D(xInteractablePos, yInteractablePos);
                    break;
                case TaskDimension.ThreeDimensional:
                    zInteractablePos = Random.Range(minRadius, maxRadius);
                    float radius = 0;
                    switch (taskRadius)
                    {
                        case TaskRadius.Low:
                            radius = zInteractablePos * Mathf.Tan(15 * Mathf.Deg2Rad);
                            break;
                        case TaskRadius.Medium:
                            radius = zInteractablePos * Mathf.Tan(30 * Mathf.Deg2Rad);
                            break;
                        case TaskRadius.High:
                            radius = zInteractablePos * Mathf.Tan(45 * Mathf.Deg2Rad);
                            break;
                    }
                    minDistanceBetweenCubes = radius;
                    xInteractablePos = Random.Range(-radius, radius);
                    yInteractablePos = Random.Range(-radius, radius);
                    ResetTargetCube3D(xInteractablePos, yInteractablePos, zInteractablePos);
                    break;
            }

            interactableCube.transform.localPosition = new Vector3(xInteractablePos, yInteractablePos, zInteractablePos);
            interactableCube.transform.localRotation = Quaternion.Euler(0, 0, 0);
            // interactableCube.transform.localScale = interactableCubeInitialScale * interactableScale;
        }

        // private void ResetTargetCube(float xPos, float yPos)
        // {
        //     float xTargetPos = 0;
        //     float yTargetPos = 0;
        //     float zTargetRotate = 0;
        //     float targetScale = 1;

        //     if (taskType == TaskType.Level3)
        //     {
        //         zTargetRotate = Random.Range(0, 90);
        //         targetScale = Random.Range(0.5f, 2f);
        //     }

        //     // Randomize the target cube position.
        //     // The new postion should not overlap with the interactable cube.
        //     do
        //     {
        //         xTargetPos = Random.Range(-distanceToCenter, distanceToCenter);
        //         yTargetPos = Random.Range(-distanceToCenter, distanceToCenter);
        //     } while (Vector3.Distance(new Vector3(xPos, yPos, 0), new Vector3(xTargetPos, yTargetPos, 0)) < minDistanceBetweenCubes
        //     && Vector3.Distance(new Vector3(xPos, yPos, 0), new Vector3(xTargetPos, yTargetPos, 0)) > minDistanceBetweenCubes * 0.5f);
            
        //     targetCube.transform.localPosition = new Vector3(xTargetPos, yTargetPos, 0);
        //     targetCube.transform.localRotation = Quaternion.Euler(0, 0, zTargetRotate);
        //     targetCube.transform.localScale = interactableCubeInitialScale * targetScale;
        // }

        private void ResetTargetCube2D(float xPos, float yPos)
        {
            float xTargetPos = 0;
            float yTargetPos = 0;

            // Randomize the target cube position.
            // The new postion should not overlap with the interactable cube.
            do
            {
                xTargetPos = Random.Range(-distanceToCenter, distanceToCenter);
                yTargetPos = Random.Range(-distanceToCenter, distanceToCenter);
            } while (Vector3.Distance(new Vector3(xPos, yPos, 0), new Vector3(xTargetPos, yTargetPos, 0)) < minDistanceBetweenCubes
            && Vector3.Distance(new Vector3(xPos, yPos, 0), new Vector3(xTargetPos, yTargetPos, 0)) > minDistanceBetweenCubes * 0.5f);
            
            targetCube.transform.localPosition = new Vector3(xTargetPos, yTargetPos, maxRadius);
            // targetCube.transform.localRotation = Quaternion.Euler(0, 0, zTargetRotate);
            // targetCube.transform.localScale = interactableCubeInitialScale * targetScale;
        }

        private void ResetTargetCube3D(float xPos, float yPos, float zPos)
        {
            float xTargetPos = 0;
            float yTargetPos = 0;
            float zTargetPos = 0;
            
            zTargetPos = Random.Range(minRadius, maxRadius);
            float radius = 0;
            switch (taskRadius)
            {
                case TaskRadius.Low:
                    radius = zTargetPos * Mathf.Tan(15 * Mathf.Deg2Rad);
                    break;
                case TaskRadius.Medium:
                    radius = zTargetPos * Mathf.Tan(30 * Mathf.Deg2Rad);
                    break;
                case TaskRadius.High:
                    radius = zTargetPos * Mathf.Tan(45 * Mathf.Deg2Rad);
                    break;
            }
            do
            {
                xTargetPos = Random.Range(-radius, radius);
                yTargetPos = Random.Range(-radius, radius);
            } while (Vector3.Distance(new Vector3(xPos, yPos, zPos), new Vector3(xTargetPos, yTargetPos, zTargetPos)) < minDistanceBetweenCubes
            && Vector3.Distance(new Vector3(xPos, yPos, zPos), new Vector3(xTargetPos, yTargetPos, zTargetPos)) > minDistanceBetweenCubes * 0.5f);

            targetCube.transform.localPosition = new Vector3(xTargetPos, yTargetPos, zTargetPos);
        }

        private void InteractionPause()
        {
            interactableCube.SetActive(false);
            targetCube.SetActive(false);
        }

        private string Evaluation()
        {
            string evaluation = "";
            Vector3 interactableCubePosition = interactableCube.transform.localPosition;
            Vector3 targetCubePosition = targetCube.transform.localPosition;

            // Calculate the distance between the cubes
            float distance = Vector3.Distance(interactableCubePosition, targetCubePosition);
            // Calculate the x, y, z distance between the cubes
            float xDistance = Mathf.Abs(interactableCubePosition.x - targetCubePosition.x);
            float yDistance = Mathf.Abs(interactableCubePosition.y - targetCubePosition.y);
            float zDistance = Mathf.Abs(interactableCubePosition.z - targetCubePosition.z);
            evaluation += "Distance: " + distance + ", xDistance: " + xDistance + ", yDistance: " + yDistance + ", zDistance: " + zDistance + "\n";

            // // Calculate the rotation difference between the cubes
            // float interactableCubeXRotation = Mathf.Abs(interactableCube.transform.localRotation.eulerAngles.x) % 90;
            // float interactableCubeYRotation = Mathf.Abs(interactableCube.transform.localRotation.eulerAngles.y) % 90;
            // float interactableCubeZRotation = Mathf.Abs(interactableCube.transform.localRotation.eulerAngles.z) % 90;
            // float targetCubeXRotation = Mathf.Abs(targetCube.transform.localRotation.eulerAngles.x) % 90;
            // float targetCubeYRotation = Mathf.Abs(targetCube.transform.localRotation.eulerAngles.y) % 90;
            // float targetCubeZRotation = Mathf.Abs(targetCube.transform.localRotation.eulerAngles.z) % 90;

            // float xRotation = Mathf.Abs(interactableCubeXRotation - targetCubeXRotation);
            // float yRotation = Mathf.Abs(interactableCubeYRotation - targetCubeYRotation);
            // float zRotation = Mathf.Abs(interactableCubeZRotation - targetCubeZRotation);
            // evaluation += "xRotation: " + xRotation + ", yRotation: " + yRotation + ", zRotation: " + zRotation;

            return evaluation;
        }
    }
}