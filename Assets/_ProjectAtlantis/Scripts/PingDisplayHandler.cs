using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PingDisplayHandler : MonoBehaviour
{
    public static PingDisplayHandler Instance;

    [SerializeField] private GameObject PingPrefab;

    [Range(0f, 2f)]
    public float CrazynessFactor = 0.1f;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
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
            pingPosition + new Vector2(Random.Range(-CrazynessFactor - speacialCrazyness.x, CrazynessFactor + speacialCrazyness.y),
                Random.Range(-CrazynessFactor, CrazynessFactor)), Quaternion.identity);
    }
}
