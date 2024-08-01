using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DottedOutline : MonoBehaviour
{
    public Transform cubeTransform;

    public void Sketch()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 16;

        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        Vector3 cubeSize = cubeTransform.localScale / 2.0f;

        // Top face corners
        Vector3 topFrontLeft = cubeTransform.position + new Vector3(-cubeSize.x, cubeSize.y, -cubeSize.z);
        Vector3 topFrontRight = cubeTransform.position + new Vector3(cubeSize.x, cubeSize.y, -cubeSize.z);
        Vector3 topBackRight = cubeTransform.position + new Vector3(cubeSize.x, cubeSize.y, cubeSize.z);
        Vector3 topBackLeft = cubeTransform.position + new Vector3(-cubeSize.x, cubeSize.y, cubeSize.z);

        // Bottom face corners
        Vector3 bottomFrontLeft = cubeTransform.position + new Vector3(-cubeSize.x, -cubeSize.y, -cubeSize.z);
        Vector3 bottomFrontRight = cubeTransform.position + new Vector3(cubeSize.x, -cubeSize.y, -cubeSize.z);
        Vector3 bottomBackRight = cubeTransform.position + new Vector3(cubeSize.x, -cubeSize.y, cubeSize.z);
        Vector3 bottomBackLeft = cubeTransform.position + new Vector3(-cubeSize.x, -cubeSize.y, cubeSize.z);

        // sketch the outline of the cube
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
