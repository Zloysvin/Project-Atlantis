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
    ObjectPool<SonarPing> sonarPingPool;

    [Range(0f, 2f)]
    public float CrazynessFactor = 0.1f;

    [Header("Sonar circle ring related:")]
    [SerializeField] private int segments = 64;
    [SerializeField] float startCircleRadius = 0f;
    LineRenderer lineRenderer;
    WaitForSeconds pingCircleUpdate = new WaitForSeconds(0.02f);

    SubmarineController player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SubmarineController>();

        sonarPingPool = new ObjectPool<SonarPing>(CreateSonarPing,OnTakePingProjectileFromPool,OnReturnPingProjectileToPool,OnDestroyPingProjectile,true,
            360,720);
    }

    public void DisplayPing(Vector2 pingPosition, Vector3 upTransform)
    {
        float2 speacialCrazyness = new float2(0, 0);

        if (CrazynessFactor > 0.5f)
        {
            speacialCrazyness.x = Mathf.Cos(CrazynessFactor);
            speacialCrazyness.y = Mathf.Sin(CrazynessFactor);
        }

        var tmpPing = sonarPingPool.Get();
        tmpPing.transform.position = pingPosition + new Vector2(
                Random.Range(-CrazynessFactor - speacialCrazyness.x, CrazynessFactor + speacialCrazyness.y),
                Random.Range(-CrazynessFactor, CrazynessFactor));

        //var tmpPing = Instantiate(PingPrefab,
        //    pingPosition + new Vector2(
        //        Random.Range(-CrazynessFactor - speacialCrazyness.x, CrazynessFactor + speacialCrazyness.y),
        //        Random.Range(-CrazynessFactor, CrazynessFactor)), Quaternion.identity).transform;

        tmpPing.transform.up = upTransform;
        //tmpPing.transform.parent = transform;

    }

    public void DisplayPing(Vector2 pingPosition)
    {
        float2 speacialCrazyness = new float2(0, 0);

        if (CrazynessFactor > 0.5f)
        {
            speacialCrazyness.x = Mathf.Cos(CrazynessFactor);
            speacialCrazyness.y = Mathf.Sin(CrazynessFactor);
        }
        var ping = sonarPingPool.Get();
        ping.transform.position = pingPosition + new Vector2(
                Random.Range(-CrazynessFactor - speacialCrazyness.x, CrazynessFactor + speacialCrazyness.y),
                Random.Range(-CrazynessFactor, CrazynessFactor));
        //Instantiate(PingPrefab,
        //    pingPosition + new Vector2(
        //        Random.Range(-CrazynessFactor - speacialCrazyness.x, CrazynessFactor + speacialCrazyness.y),
        //        Random.Range(-CrazynessFactor, CrazynessFactor)), Quaternion.identity);
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
        if (!player.EngineOn)
        {
            lineRenderer.enabled = false;
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

    #region SonarPingPool

    private SonarPing CreateSonarPing()
    {
        SonarPing ping = Instantiate(PingPrefab, transform.position, Quaternion.identity);
        ping.SetPool(sonarPingPool);
        ping.transform.parent = transform;

        return ping;
    }

    private void OnTakePingProjectileFromPool(SonarPing ping)
    {
        //ping.transform.position = transform.position;
        //ping.transform.rotation = Quaternion.identity;

        ping.gameObject.SetActive(true);
    }

    private void OnReturnPingProjectileToPool(SonarPing ping)
    {
        ping.gameObject.SetActive(false);
    }

    // Whats happening when the object is destroyed instead of being send back to the pool.
    private void OnDestroyPingProjectile(SonarPing ping)
    {
        Destroy(ping.gameObject);
    }

    #endregion
}
