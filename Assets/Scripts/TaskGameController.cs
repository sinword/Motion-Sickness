using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TaskRange
{
    Low,
    Medium,
    High
}

public enum state
{
    Interation,
    Pause
}

public class TaskGameController : MonoBehaviour
{
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
    private float roundTime = 5f;
    [SerializeField]
    private float pauseTime = 1f;
    private state state = state.Interation;
    private float timeLeft;
    private string report;
    private Vector3 interactableCubeInitialScale;
    private float distanceToCenterLow;
    private float distanceToCenterMedium;
    private float distanceToCenterHigh;
    void Start()
    {
        timeLeft = roundTime;
        interactableCubeInitialScale = interactableCube.transform.localScale;
        ResetPositions();
        // 30 degrees FOV
        distanceToCenterLow = interactionArea.transform.position.z * Mathf.Tan(15 * Mathf.Deg2Rad);
        // 60 degrees FOV
        distanceToCenterMedium = interactionArea.transform.position.z * Mathf.Tan(30 * Mathf.Deg2Rad);
        // 90 degrees FOV
        distanceToCenterHigh = interactionArea.transform.position.z * Mathf.Tan(45 * Mathf.Deg2Rad);
        
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        timeText.text = ((int)timeLeft).ToString();
        switch (state)
        {
            case state.Interation:
                if (timeLeft <= 0)
                {
                    report = Evaluation();
                    Debug.LogWarning(report);
                    InteractionPause();
                    timeLeft = pauseTime;
                    state = state.Pause;
                }
                break;
            case state.Pause:
                if (timeLeft <= 0)
                    {
                        ResetPositions();
                        timeLeft = roundTime;
                        state = state.Interation;
                    }
                    break;
        }
        
    }

    private void ResetPositions()
    {
        float distanceToCenter = 0f;
        float minDistanceBetweenCubes = 0f;
        interactableCube.SetActive(true);
        targetCube.SetActive(true);

        switch (taskRange)
        {
            case TaskRange.Low:
                distanceToCenter = distanceToCenterLow;
                minDistanceBetweenCubes = distanceToCenterLow;
                break;
            case TaskRange.Medium:
                distanceToCenter = distanceToCenterMedium;
                minDistanceBetweenCubes = distanceToCenterMedium;
                break;
            case TaskRange.High:
                distanceToCenter = distanceToCenterHigh;
                minDistanceBetweenCubes = distanceToCenterHigh;
                break;
        }

        // Randomize the position of the interactable and target cubes in the circle
        // The Euclidean distance between the cubes and the center of the circle should be less than distanceToCenter
        // The cubes should not overlap

        Vector3 interactableCubeLocalPos = GetRandomPositionWithinCircle(taskRange, distanceToCenter);
        Vector3 targetCubeLocalPos;
        do
        {
            targetCubeLocalPos = GetRandomPositionWithinCircle(taskRange, distanceToCenter);
        } while (Vector3.Distance(interactableCubeLocalPos, targetCubeLocalPos) <= minDistanceBetweenCubes);

        interactableCube.transform.localPosition = interactableCubeLocalPos;
        // Reset the rotation and size of the interactable cube
        interactableCube.transform.localRotation = Quaternion.identity;
        interactableCube.transform.localScale = interactableCubeInitialScale;
        targetCube.transform.localPosition = targetCubeLocalPos;

        LineRenderer outline = targetCube.transform.Find("DottedOutline").GetComponent<LineRenderer>();
        outline.GetComponent<DottedOutline>().Sketch();
    }

    private Vector3 GetRandomPositionWithinCircle(TaskRange taskRange, float radius)
    {
        
        float x = 0;
        float y = 0;
        float distance;
        int xMultiplier, yMultiplier;
        do
        {
            switch (taskRange)
            {
                case TaskRange.Low:
                    x = Random.Range(-1 * distanceToCenterLow, distanceToCenterLow);
                    y = Random.Range(-1 * distanceToCenterLow, distanceToCenterLow);
                    break;
                case TaskRange.Medium:
                    x = Random.Range(-1 * distanceToCenterMedium, distanceToCenterMedium);
                    xMultiplier = Random.Range(0,2) * 2 - 1;
                    x *= xMultiplier;
                    y = Random.Range(-1 * distanceToCenterMedium, distanceToCenterMedium);
                    yMultiplier = Random.Range(0,2) * 2 - 1;
                    y *= yMultiplier;
                    break;
                case TaskRange.High:
                    x = Random.Range(-1 * distanceToCenterHigh, distanceToCenterHigh);
                    xMultiplier = Random.Range(0, 2) * 2 - 1;
                    x *= xMultiplier;
                    y = Random.Range(-1 * distanceToCenterHigh, distanceToCenterHigh);
                    yMultiplier = Random.Range(0, 2) * 2 - 1;
                    y *= yMultiplier;
                    break;
            }
            distance = Mathf.Sqrt(x * x + y * y);
            } while (distance > radius);
        return new Vector3(x, y, 0);
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
        float xRotation = Mathf.Abs(interactableCube.transform.localRotation.eulerAngles.x - targetCube.transform.localRotation.eulerAngles.x);
        float yRotation = Mathf.Abs(interactableCube.transform.localRotation.eulerAngles.y - targetCube.transform.localRotation.eulerAngles.y);
        float zRotation = Mathf.Abs(interactableCube.transform.localRotation.eulerAngles.z - targetCube.transform.localRotation.eulerAngles.z);
        evaluation += "xRotation: " + xRotation + ", yRotation: " + yRotation + ", zRotation: " + zRotation;

        return evaluation;
    }
}

