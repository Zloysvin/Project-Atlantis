using UnityEngine;

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
        Instantiate(PingPrefab,
            pingPosition + new Vector2(Random.Range(-CrazynessFactor, CrazynessFactor),
                Random.Range(-CrazynessFactor, CrazynessFactor)), Quaternion.identity);
    }
}
