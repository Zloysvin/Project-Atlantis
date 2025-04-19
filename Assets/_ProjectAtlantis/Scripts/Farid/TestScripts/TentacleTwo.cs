using UnityEngine;

public class TentacleTwo : MonoBehaviour
{
    public int length = 30;
    public LineRenderer lineRenderer;
    public Vector3[] segmentPositions;
    Vector3[] segmentVelocities;
    public Transform targetDirection;
    public float targetDistance = 0.2f;
    public float smoothSpeed = 0.02f;

    public float wiggleSpeed = 10;
    public float wiggleMagnitude = 20;
    public Transform wiggleDirection;

    public Transform[] bodyParts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = length;
        segmentPositions = new Vector3[length];
        segmentVelocities = new Vector3[length];

        ResetPose();
    }

    // Update is called once per frame
    void Update()
    {
        wiggleDirection.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPositions[0] = targetDirection.position;
        for (int i = 1; i < segmentPositions.Length; i++)
        {
            Vector3 targetPos = segmentPositions[i-1] + (segmentPositions[i] - segmentPositions[i-1]).normalized * targetDistance;
            segmentPositions[i] = Vector3.SmoothDamp(segmentPositions[i],targetPos,ref segmentVelocities[i],smoothSpeed);

            bodyParts[i-1].position = segmentPositions[i];

        }

        lineRenderer.SetPositions(segmentPositions);
    }

    void ResetPose()
    {
        segmentPositions[0] = targetDirection.position;
        for (int i = 1; i < length; i++)
        {
            segmentPositions[i] = segmentPositions[i - 1] + targetDirection.right * targetDistance;
        }
        lineRenderer.SetPositions(segmentPositions);
    }
}
