using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System;
using UnityEngine.Pool;

public class PingDisplayHandler : MonoBehaviour
{
    public static PingDisplayHandler Instance;

    [SerializeField] private SonarPing PingPrefab;

    [Range(0f, 2f)]
    public float CrazynessFactor = 0.1f;

    [Header("Sonar circle ring related:")]
    [SerializeField] private int segments = 64;
    [SerializeField] float startCircleRadius = 0f;
    LineRenderer lineRenderer;
    WaitForSeconds pingCircleUpdate = new WaitForSeconds(0.02f);

    private Vector3[] expansionDirections;

    SubmarineController player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SubmarineController>();

        expansionDirections = new Vector3[segments];

        float directionAngleChange = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            expansionDirections[i] = Quaternion.AngleAxis(i * directionAngleChange, Vector3.forward) * Vector2.up;
        }
    }

    public void DisplayPing(Vector2 pingPosition, Vector3 upTransform)
    {
        float2 speacialCrazyness = new float2(0, 0);

        if (CrazynessFactor > 0.5f)
        {
            speacialCrazyness.x = Mathf.Cos(CrazynessFactor);
            speacialCrazyness.y = Mathf.Sin(CrazynessFactor);
        }

        var tmpPing = Instantiate(PingPrefab,
            pingPosition + new Vector2(
                Random.Range(-CrazynessFactor - speacialCrazyness.x, CrazynessFactor + speacialCrazyness.y),
                Random.Range(-CrazynessFactor, CrazynessFactor)), Quaternion.identity).transform;

        tmpPing.transform.up = upTransform;
        tmpPing.transform.parent = transform;

    }

    public void DisplayPing(Vector2 pingPosition)
    {
        float2 speacialCrazyness = new float2(0, 0);

        if (CrazynessFactor > 0.5f)
        {
            speacialCrazyness.x = Mathf.Cos(CrazynessFactor);
            speacialCrazyness.y = Mathf.Sin(CrazynessFactor);
        }

        Instantiate(PingPrefab,
            pingPosition + new Vector2(
                Random.Range(-CrazynessFactor - speacialCrazyness.x, CrazynessFactor + speacialCrazyness.y),
                Random.Range(-CrazynessFactor, CrazynessFactor)), Quaternion.identity);
    }

    public void DisplaySonarRing(float sonarPingDuration, float circleSpeed,Vector2 startPos)
    {
        StopAllCoroutines();
        StartCoroutine(SonarCircle(sonarPingDuration, circleSpeed,startPos));
    }

    private IEnumerator SonarCircle(float sonarPingDuration, float circleSpeed,Vector2 startPos)
    {
        lineRenderer.enabled = true;
        float timer = sonarPingDuration;
        //float currenCircleRadius = startCircleRadius;

        Vector3[] segmentPositions = new Vector3[segments + 1];
        for (int i = 0; i < segments; i++)
        {
            segmentPositions[i] = startPos;
        }

        while (timer > 0f)
        {
            for (int i = 0; i < segments; i++)
            {
                segmentPositions[i] += expansionDirections[i] * circleSpeed * Time.deltaTime;
            }

            segmentPositions[segments] = segmentPositions[0];

            lineRenderer.SetPositions(segmentPositions);

            timer -= Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i <= segments; i++)
        {
            lineRenderer.SetPosition(i, Vector3.zero);
        }
    }

    void DrawCircle(LineRenderer lineRenderer, float radius, int segments = 64)
    {
        lineRenderer.positionCount = segments + 1;
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += 2 * Mathf.PI / segments;
        }
    }
}
