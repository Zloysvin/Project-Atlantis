using UnityEngine;

public class AudioManagerF : MonoBehaviour
{
    public static AudioManagerF Instance;
    [SerializeField] AudioSource uiSource;
    [SerializeField] AudioSource logSource;

    [Header("UI clips related:")]
    [SerializeField] AudioClip buttonPressedClip;
    [SerializeField] AudioClip buttonRejectedClip;
    [SerializeField] AudioClip buttonHoveredClip;
    [SerializeField] AudioClip beaconDroppedClip;
    [SerializeField] AudioClip logEntryNormalClip;
    [SerializeField] AudioClip logEntryWarningClip;
    [SerializeField] AudioClip logEntryDangerClip;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else Destroy(this);
    }

    public void PlayUIAuidoClip(AudioClip clip)
    {
        uiSource.clip = clip;
        uiSource.Play();
    }

    #region QuickAccessMethods
    public void PlayButtonPressedSound()
    {
        uiSource.clip = buttonPressedClip;
        uiSource.Play();
    }
    public void PlayButtonRejectedSound()
    {
        uiSource.clip = buttonRejectedClip;
        uiSource.Play();
    }
    public void PlayButtonHoveredSound()
    {
        uiSource.clip = buttonHoveredClip;
        uiSource.Play();
    }
    public void PlayBeaconSound()
    {
        uiSource.clip = beaconDroppedClip;
        uiSource.Play();
    }
    public void PlayLogEntryNormalSound()
    {
        logSource.clip = logEntryNormalClip;
        logSource.Play();
    }
    public void PlayLogEntryWarningSound()
    {
        logSource.clip = logEntryWarningClip;
        logSource.Play();
    }
    public void PlayLogEntryDangerSound()
    {
        logSource.clip = logEntryDangerClip;
        logSource.Play();
    }
    #endregion
}
