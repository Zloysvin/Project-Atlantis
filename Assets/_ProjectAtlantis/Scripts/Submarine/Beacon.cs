using System.Collections;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    private string beaconCoordinates;

    public float floatingDuration = 4f;
    public Vector2 floatRange = new Vector2(0.3f, 1f);
    public float floatSpeed = 2f;

    public void LaunchBeacon(Vector2 floatDirection, string beaconCoordinates)
    {
        this.beaconCoordinates = beaconCoordinates;
        StartCoroutine(Launch(floatDirection));
    }

    private IEnumerator Launch(Vector2 floatDirection)
    {
        Vector2 targetPos = (Vector2)transform.position + floatDirection * Random.Range(floatRange.x, floatRange.y);
        float timer = floatingDuration;
        while (timer > 0f)
        {
            timer = Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, floatSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private void OnMouseEnter()
    {
        LogEntryController.Instance.UpdateCurrentLogLine($"[Beacon at: {beaconCoordinates}]");
    }

    private void OnMouseExit()
    {
        LogEntryController.Instance.ClearCurrentLogLine();
    }
}