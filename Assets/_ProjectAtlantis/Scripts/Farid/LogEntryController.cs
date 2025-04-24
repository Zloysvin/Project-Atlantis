using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class LogEntryController : MonoBehaviour
{
    public static LogEntryController Instance;
    [SerializeField] Transform log;
    [SerializeField] TextMeshProUGUI currentLogLine;
    [SerializeField] Color logNormalColor = Color.green;
    [SerializeField] Color logWarningColor = Color.yellow;
    [SerializeField] Color logDangerColor = Color.red;

    int currentLogIndex = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space)) { AddLogEntry("ddsds"); }
    }
    public void AddLogEntry(string entry,LogEntryMode mode = LogEntryMode.Normal)
    {
        Transform currentEntry = log.GetChild(log.childCount - 1);
        TextMeshProUGUI text = currentEntry.GetComponent<TextMeshProUGUI>();
        
        Color c = Color.white;
        switch (mode)
        {
            case LogEntryMode.Normal:
                c = logNormalColor;
                break;
            case LogEntryMode.Warning:
                c= logWarningColor;
                break;
            case LogEntryMode.Danger:
                c= logDangerColor;
                break;
            default:
                break;
        }

        text.text = $"<color={ColorToHex(c)}>{entry}</color>";
        currentEntry.SetAsFirstSibling();
    }
    public void UpdateCurrentLogLine(string text, LogEntryMode mode = LogEntryMode.Normal)
    {
        Color c = Color.white;
        switch (mode)
        {
            case LogEntryMode.Normal:
                c = logNormalColor;
                break;
            case LogEntryMode.Warning:
                c = logWarningColor;
                break;
            case LogEntryMode.Danger:
                c = logDangerColor;
                break;
            default:
                break;
        }
        currentLogLine.text = $"<color={ColorToHex(c)}>{text}</color>";
    }
    private string ColorToHex(Color color)
    {
        Color32 c = color;
            return $"#{c.r:X2}{c.g:X2}{c.b:X2}";
    }
    public void ClearCurrentLogLine()
    {
        currentLogLine.text = "[...]";
    }
}
public enum LogEntryMode { Normal, Warning, Danger }
