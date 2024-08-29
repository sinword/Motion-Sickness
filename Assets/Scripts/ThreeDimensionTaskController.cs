using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

public enum TaskDistance
{
    Sixty,
    Eighty
}

public enum TaskRange
{
    Low,
    Medium,
    High
}

public enum TaskType
{
    Level1,
    Level2,
    Level3
}

// Level1: Select
// Level2: Drag and drop
// Level3: Drag and drop, rotate, scale

public enum State
{
    Interaction,
    Pause
}

public class ThreeDimensionTaskController : MonoBehaviour
{
    [SerializeField]
    private TaskDistance taskDistance;
    [SerializeField]
    private GameObject interactionArea;
    [SerializeField]
    private GameObject interactableCube;

    [SerializeField]
    private GameObject targetCube;

    [SerializeField]
    private TextMeshProUGUI timeText;   

    // Select task bar
    [SerializeField]
    private TaskRange taskRange;
    [SerializeField]
    private TaskType taskType;

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
        switch (taskDistance)
        {
            case TaskDistance.Sixty:
                interactionArea.transform.position = new Vector3(0, 1, 0.4243f);
                break;
            case TaskDistance.Eighty:
                interactionArea.transform.position = new Vector3(0, 1, 0.5657f);
                break;
        }
    }

    void Start()
    {
        timeLeft = roundTime;
        interactableCubeInitialScale = interactableCube.transform.localScale;
        twoGrabFreeTransformer = interactableCube.GetComponent<TwoGrabFreeTransformer>();

        if (taskType == TaskType.Level1)
        {
            targetCube.SetActive(false);
        }
        ResetInteractableCube();
        switch (taskRange)
        {
            case TaskRange.Low:
                // 30 degree FOV
                distanceToCenter = interactionArea.transform.position.z * Mathf.Tan(15 * Mathf.Deg2Rad);
                break;
            case TaskRange.Medium:
                // 60 degree FOV
                distanceToCenter = interactionArea.transform.position.z * Mathf.Tan(30 * Mathf.Deg2Rad);
                break;
            case TaskRange.High:
                // 90 degree FOV
                distanceToCenter = interactionArea.transform.position.z * Mathf.Tan(45 * Mathf.Deg2Rad);
                break;
        }
        minDistanceBetweenCubes = distanceToCenter;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        timeText.text = ((int)timeLeft).ToString();

        switch (taskType)
        {
            case TaskType.Level1:
                // Disable two grab free transformer script
                
                if (twoGrabFreeTransformer != null)
                {
                    twoGrabFreeTransformer.enabled = false;
                }
                break;
            case TaskType.Level2:
                if (twoGrabFreeTransformer != null)
                {
                    twoGrabFreeTransformer.enabled = false;
                }
                else
                {
                    Debug.LogWarning("TwoGrabFreeTransformer is null");
                }
                break;
            case TaskType.Level3:
                if (twoGrabFreeTransformer != null)
                {
                    twoGrabFreeTransformer.enabled = true;
                    interactableCube.transform.localRotation = Quaternion.Euler(0, 0, interactableCube.transform.localRotation.eulerAngles.z);
                }
                break;
        }
        
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
        if (taskType == TaskType.Level3)
        {
            interactableCube.transform.localRotation = Quaternion.Euler(0, 0, interactableCube.transform.localRotation.eulerAngles.z);
        }
    }

    private void ResetInteractableCube()
    {
        float xInteractablePos = 0;
        float yInteractablePos = 0;
        float zInteractableRotate = 0;
        float interactableScale = 1;
        interactableCube.SetActive(true);
        targetCube.SetActive(true);

        switch (taskType)
        {
            case TaskType.Level1:
                // Task: Select
                xInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
                yInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
                break;
            case TaskType.Level2:
                // Task: Drag and drop
                xInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
                yInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
                ResetTargetCube(xInteractablePos, yInteractablePos);
                break;
            case TaskType.Level3:
                // Task: Drag and drop, rotate, scale
                xInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
                yInteractablePos = Random.Range(-distanceToCenter, distanceToCenter);
                zInteractableRotate = Random.Range(0, 90);
                interactableScale = Random.Range(0.5f, 2f);
                ResetTargetCube(xInteractablePos, yInteractablePos);
                break;
        }

        interactableCube.transform.localPosition = new Vector3(xInteractablePos, yInteractablePos, 0);
        interactableCube.transform.localRotation = Quaternion.Euler(0, 0, zInteractableRotate);
        interactableCube.transform.localScale = interactableCubeInitialScale * interactableScale;
    }

    private void ResetTargetCube(float xPos, float yPos)
    {
        float xTargetPos = 0;
        float yTargetPos = 0;
        float zTargetRotate = 0;
        float targetScale = 1;

        if (taskType == TaskType.Level3)
        {
            zTargetRotate = Random.Range(0, 90);
            targetScale = Random.Range(0.5f, 2f);
        }

        // Randomize the target cube position.
        // The new postion should not overlap with the interactable cube.
        do
        {
            xTargetPos = Random.Range(-distanceToCenter, distanceToCenter);
            yTargetPos = Random.Range(-distanceToCenter, distanceToCenter);
        } while (Vector3.Distance(new Vector3(xPos, yPos, 0), new Vector3(xTargetPos, yTargetPos, 0)) < minDistanceBetweenCubes
        && Vector3.Distance(new Vector3(xPos, yPos, 0), new Vector3(xTargetPos, yTargetPos, 0)) > minDistanceBetweenCubes * 0.5f);
        
        targetCube.transform.localPosition = new Vector3(xTargetPos, yTargetPos, 0);
        targetCube.transform.localRotation = Quaternion.Euler(0, 0, zTargetRotate);
        targetCube.transform.localScale = interactableCubeInitialScale * targetScale;
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

        // Calculate the rotation difference between the cubes
        float interactableCubeXRotation = Mathf.Abs(interactableCube.transform.localRotation.eulerAngles.x) % 90;
        float interactableCubeYRotation = Mathf.Abs(interactableCube.transform.localRotation.eulerAngles.y) % 90;
        float interactableCubeZRotation = Mathf.Abs(interactableCube.transform.localRotation.eulerAngles.z) % 90;
        float targetCubeXRotation = Mathf.Abs(targetCube.transform.localRotation.eulerAngles.x) % 90;
        float targetCubeYRotation = Mathf.Abs(targetCube.transform.localRotation.eulerAngles.y) % 90;
        float targetCubeZRotation = Mathf.Abs(targetCube.transform.localRotation.eulerAngles.z) % 90;

        float xRotation = Mathf.Abs(interactableCubeXRotation - targetCubeXRotation);
        float yRotation = Mathf.Abs(interactableCubeYRotation - targetCubeYRotation);
        float zRotation = Mathf.Abs(interactableCubeZRotation - targetCubeZRotation);
        evaluation += "xRotation: " + xRotation + ", yRotation: " + yRotation + ", zRotation: " + zRotation;

        return evaluation;
    }
}

/*
Version 1
To the outer circle 60 cm
Theta1 = 15 degrees
Distance to the center of the circle = 60 * sin(15) = 15.52 cm
Theta2 = 30 degrees
Distance to the center of the circle = 60 * sin(30) = 30 cm
Theta3 = 45 degrees
Distance to the center of the circle = 60 * sin(45) = 42.42 cm

Version 2
To the outer circle 80 cm
Theta1 = 15 degrees
Distance to the center of the circle = 80 * sin(15) = 20.69 cm 
Theta2 = 30 degrees
Distance to the center of the circle = 80 * sin(30) = 40 cm
Theta3 = 45 degrees
Distance to the center of the circle = 80 * sin(45) = 56.57 cm
*/