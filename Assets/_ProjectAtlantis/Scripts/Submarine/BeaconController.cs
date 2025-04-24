using TMPro;
using UnityEngine;

public class BeaconController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI beaconDisplay;
    [SerializeField] private int maxBeacons = 15;
    [SerializeField] private Beacon beaconPrefab;

    private int currentBeacons;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentBeacons = maxBeacons;
        currentBeacons = maxBeacons;
        beaconDisplay.text = "x" + currentBeacons;
    }

    public void EjectBeacon()
    {
        if (currentBeacons == 0) { return; }

        currentBeacons--;
        if (currentBeacons <= 0)
        {
            beaconDisplay.color = Color.red;
        }
        else if (currentBeacons < 4)
        {
            beaconDisplay.color = Color.yellow;
        }
        else
        {
            beaconDisplay.color = Color.green;
        }
        beaconDisplay.text = "x" + currentBeacons;
        LogEntryController.Instance.AddLogEntry($"Launched beacon at: {StatsFaker.Instance.GetCurrentCoordinates()}");
        Instantiate(beaconPrefab, transform.position, Quaternion.identity).LaunchBeacon(-rb.linearVelocity.normalized, StatsFaker.Instance.GetCurrentCoordinates()); ;
    }

    public void DisplayCurrentLogLine()
    {
        if (currentBeacons <= 0)
        {
            LogEntryController.Instance.UpdateCurrentLogLine("[No beacons left!]", LogEntryMode.Danger);
        }
        else if (currentBeacons < 4)
        {
            LogEntryController.Instance.UpdateCurrentLogLine($"[Launch beacon at: {StatsFaker.Instance.GetCurrentCoordinates()}]", LogEntryMode.Warning);
        }
        else
        {
            LogEntryController.Instance.UpdateCurrentLogLine($"[Launch beacon at: {StatsFaker.Instance.GetCurrentCoordinates()}]");
        }
    }
}