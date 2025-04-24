using UnityEngine;
using System.Collections;
public class PingDisplayHandler : MonoBehaviour
{
    public static PingDisplayHandler Instance;

    [SerializeField] private GameObject PingPrefab;

    [Range(0f, 2f)]
    public float CrazynessFactor = 0.1f;

    [Header("Sonar circle ring related:")]
    [SerializeField] private int segments = 64;
    [SerializeField] float startCircleRadius = 0f;
    LineRenderer lineRenderer;
    WaitForSeconds pingCircleUpdate = new WaitForSeconds(0.02f);
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
    }

    public void DisplayPing(Vector2 pingPosition, Vector3 upTransform)
    {
        var tmpPing = Instantiate(PingPrefab,
            pingPosition + new Vector2(Random.Range(-CrazynessFactor, CrazynessFactor),
                Random.Range(-CrazynessFactor, CrazynessFactor)), Quaternion.identity).transform;

        tmpPing.up = upTransform;
        tmpPing.parent = transform;

    }

    public void DisplaySonarRing(float sonarPingDuration, float circleSpeed,Vector2 startPos)
    {
        StopAllCoroutines();
        StartCoroutine(SonarCircle(sonarPingDuration, circleSpeed,startPos));
    }

    private IEnumerator SonarCircle(float sonarPingDuration, float circleSpeed,Vector2 startPos)
    {

        float timer = sonarPingDuration;
        float currenCircleRadius = startCircleRadius;
        while (timer > 0f)
        {
            float angle = 0f;
            
            for (int i = 0; i < segments; i++)
            {
                float x = Mathf.Cos(angle) * currenCircleRadius + startPos.x;
                float y = Mathf.Sin(angle) * currenCircleRadius + startPos.y;
                lineRenderer.SetPosition(i, new Vector3(x, y, 0));
                angle += 2 * Mathf.PI / segments;
            }
            lineRenderer.SetPosition(lineRenderer.positionCount-1, 
                new Vector3(Mathf.Cos(0) * currenCircleRadius + startPos.x, Mathf.Sin(0) * currenCircleRadius + startPos.y, 0));
            currenCircleRadius += circleSpeed * Time.deltaTime;
            timer -= 0.02f;
            yield return pingCircleUpdate;
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
