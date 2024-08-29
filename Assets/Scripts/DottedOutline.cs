using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DottedOutline : MonoBehaviour
{
    public Transform cubeTransform;
    private LineRenderer lineRenderer;

    private void Start()
    {
        // Initialize and configure LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = false;  // Do not loop the line
        lineRenderer.useWorldSpace = false; // Use local space since the GameObject is parented under the cube
        lineRenderer.positionCount = 16; // Update to 16 since we need to draw the entire outline
        
        // Call the function to sketch the outline
        Sketch();
    }

    public void Sketch()
    {
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        float length = 0.5f;

        // Calculate the corner positions in the local space of the cube
        Vector3 topFrontLeft = new Vector3(-length, length, length);
        Vector3 topFrontRight = new Vector3(length, length, length);
        Vector3 topBackRight = new Vector3(length, length, -length);
        Vector3 topBackLeft = new Vector3(-length, length, -length);
        Vector3 bottomFrontLeft = new Vector3(-length, -length, length);
        Vector3 bottomFrontRight = new Vector3(length, -length, length);
        Vector3 bottomBackRight = new Vector3(length, -length, -length);
        Vector3 bottomBackLeft = new Vector3(-length, -length, -length);

        // Assign positions for the outline of the cube in local space
        positions[0] = topFrontLeft;
        positions[1] = topFrontRight;
        positions[2] = topBackRight;
        positions[3] = topBackLeft;
        positions[4] = topFrontLeft;
        positions[5] = bottomFrontLeft;
        positions[6] = bottomFrontRight;
        positions[7] = topFrontRight;
        positions[8] = topBackRight;
        positions[9] = bottomBackRight;
        positions[10] = bottomFrontRight;
        positions[11] = bottomBackRight;
        positions[12] = bottomBackLeft;
        positions[13] = bottomFrontLeft;
        positions[14] = bottomBackLeft;
        positions[15] = topBackLeft;

        lineRenderer.SetPositions(positions);
    }
}
