using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    public static NarrationManager Instance;
    private int hullWarningIndex = 0;
    private int energyWarningIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void HullWarning(int hull)
    {
        if (hull <= 25 && hullWarningIndex == 2)
        {
            LogEntryController.Instance.AddLogEntry("Hull Critical! 25% Hull Integrity Left!", LogEntryMode.Danger);
            hullWarningIndex++;
        }
        else if (hull <= 50 && hullWarningIndex == 1)
        {
            LogEntryController.Instance.AddLogEntry("Hull Damaged! 50% Hull Integrity Left!", LogEntryMode.Warning);
            hullWarningIndex++;
        }
        else if (hull <= 75 && hullWarningIndex == 0)
        {
            LogEntryController.Instance.AddLogEntry("Hull Damaged! 75% Hull Integrity Left!", LogEntryMode.Warning);
            hullWarningIndex++;
        }
    }

    public void EnergyWarning(float energy)
    {
        if (energy <= 25 && energyWarningIndex == 2)
        {
            LogEntryController.Instance.AddLogEntry("Energy Critical! 25% Battery Charge Left!", LogEntryMode.Danger);
            energyWarningIndex++;
        }
        else if (energy <= 50 && energyWarningIndex == 1)
        {
            LogEntryController.Instance.AddLogEntry("Energy Status: 50% Battery Charge Left!", LogEntryMode.Warning);
            energyWarningIndex++;
        }
        else if (energy <= 75 && energyWarningIndex == 0)
        {
            LogEntryController.Instance.AddLogEntry("Energy Status: 75% Battery Charge Left!", LogEntryMode.Warning);
            energyWarningIndex++;
        }
    }
}
