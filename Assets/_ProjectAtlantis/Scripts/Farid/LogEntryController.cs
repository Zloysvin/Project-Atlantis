using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogEntryController : MonoBehaviour
{
    public static LogEntryController Instance;

    [SerializeField] private List<TMP_Text> log;
    [SerializeField] private Color logNormalColor = Color.green;
    [SerializeField] private Color logWarningColor = Color.yellow;
    [SerializeField] private Color logDangerColor = Color.red;

    private Dictionary<LogEntryMode, Color> EntryColorMatrix = new Dictionary<LogEntryMode, Color> {
        { LogEntryMode.Normal, Color.green},
        { LogEntryMode.Warning, Color.yellow},
        { LogEntryMode.Danger, Color.red}
    };

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
            Destroy(this);
    }


    public void AddLogEntry(string entry, LogEntryMode mode = LogEntryMode.Normal)
    {
        for (int i = log.Count - 1; i > 0; i--)
        {
            log[i].text = log[i - 1].text;
        }

        log[0].text = "";

        Color c = EntryColorMatrix[mode];

        TypeWriter.Instance.StartTypeWriter(log[0], $"<color={ColorToHex(c)}>{entry}</color>");
    }

    //public void UpdateCurrentLogLine(string text, LogEntryMode mode = LogEntryMode.Normal)
    //{
    //    Color c = Color.white;
    //    switch (mode)
    //    {
    //        case LogEntryMode.Normal:
    //            c = logNormalColor;
    //            break;

    //        case LogEntryMode.Warning:
    //            c = logWarningColor;
    //            break;

    //        case LogEntryMode.Danger:
    //            c = logDangerColor;
    //            break;

    //        default:
    //            break;
    //    }
    //    currentLogLine.text = $"<color={ColorToHex(c)}>{text}</color>";
    //}
    //public void UpdateCurrentLogLine(string text)
    //{
    //    currentLogLine.text = $"<color={ColorToHex(logNormalColor)}>{text}</color>";
    //}

    private string ColorToHex(Color color)
    {
        Color32 c = color;
        return $"#{c.r:X2}{c.g:X2}{c.b:X2}";
    }

    //public void ClearCurrentLogLine()
    //{
    //    currentLogLine.text = "[...]";
    //}

    #region QuickMethods

    //public void AddLogEntryNormal(string entry)
    //{
    //    Transform currentEntry = log.GetChild(log.childCount - 1);
    //    TextMeshProUGUI text = currentEntry.GetComponent<TextMeshProUGUI>();

    //    text.text = $"<color={ColorToHex(logNormalColor)}>{entry}</color>";
    //    currentEntry.SetAsFirstSibling();
    //}
    //public void AddLogEntryWarning(string entry)
    //{
    //    Transform currentEntry = log.GetChild(log.childCount - 1);
    //    TextMeshProUGUI text = currentEntry.GetComponent<TextMeshProUGUI>();


    //    text.text = $"<color={ColorToHex(logWarningColor)}>{entry}</color>";
    //    currentEntry.SetAsFirstSibling();

    //    AudioManagerF.Instance.PlayLogEntryWarningSound();
    //}
    //public void AddLogEntryDanger(string entry)
    //{
    //    Transform currentEntry = log.GetChild(log.childCount - 1);
    //    TextMeshProUGUI text = currentEntry.GetComponent<TextMeshProUGUI>();

    //    text.text = $"<color={ColorToHex(logDangerColor)}>{entry}</color>";
    //    currentEntry.SetAsFirstSibling();

    //    AudioManagerF.Instance.PlayLogEntryDangerSound();
    //}


    #endregion


}

public enum LogEntryMode
{ Normal, Warning, Danger }