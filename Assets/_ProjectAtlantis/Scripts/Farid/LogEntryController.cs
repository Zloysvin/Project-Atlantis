using TMPro;
using UnityEngine;

public class LogEntryController : MonoBehaviour
{
    public static LogEntryController Instance;
    [SerializeField] private Transform log;
    [SerializeField] private TextMeshProUGUI currentLogLine;
    [SerializeField] private Color logNormalColor = Color.green;
    [SerializeField] private Color logWarningColor = Color.yellow;
    [SerializeField] private Color logDangerColor = Color.red;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
            Destroy(this);
    }

    public void AddLogEntry(string entry, LogEntryMode mode = LogEntryMode.Normal)
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
                c = logWarningColor;
                break;

            case LogEntryMode.Danger:
                c = logDangerColor;
                break;

            default:
                break;
        }

        text.text = $"<color={ColorToHex(c)}>{entry}</color>";
        currentEntry.SetAsFirstSibling();

        AudioManagerF.Instance.PlayLogEntryNormalSound();
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

public enum LogEntryMode
{ Normal, Warning, Danger }