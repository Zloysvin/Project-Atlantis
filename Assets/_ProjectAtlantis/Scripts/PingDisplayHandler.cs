using UnityEngine;

public class PingDisplayHandler : MonoBehaviour
{
    public static PingDisplayHandler Instance;

    [SerializeField] private GameObject PingPrefab;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void DisplayPing(Vector2 pingPosition)
    {
        Instantiate(PingPrefab, pingPosition, Quaternion.identity);
    }
}
