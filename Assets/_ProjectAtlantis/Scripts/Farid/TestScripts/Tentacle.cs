using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public int length = 30;
    public LineRenderer lineRenderer;
    public Vector3[] segmentPositions;
    Vector3[] segmentVelocities;
    public Transform targetDirection;
    public float targetDistance = 0.2f;
    public float smoothSpeed = 0.02f;
    public float trailSpeed = 350f;

    public float wiggleSpeed = 10;
    public float wiggleMagnitude = 20;
    public Transform wiggleDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = length;
        segmentPositions = new Vector3[length];
        segmentVelocities = new Vector3[length];
    }

    // Update is called once per frame
    void Update()
    {
        wiggleDirection.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPositions[0] = targetDirection.position;
        for (int i = 1; i < segmentPositions.Length; i++)
        {
            segmentPositions[i] = Vector3.SmoothDamp(segmentPositions[i], segmentPositions[i-1] + targetDirection.right * targetDistance,ref segmentVelocities[i],smoothSpeed + i/trailSpeed);
        }

        lineRenderer.SetPositions(segmentPositions);
    }
}
