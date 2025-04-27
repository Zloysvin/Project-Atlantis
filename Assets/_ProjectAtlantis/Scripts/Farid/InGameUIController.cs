using DG.Tweening;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    [Header("TopBar:")]
    [SerializeField] TextMeshProUGUI energyInfo;
    [SerializeField] TextMeshProUGUI speedInfo;
    [SerializeField] TextMeshProUGUI timeInfo;
    [SerializeField] TextMeshProUGUI temperatureInfo;
    [SerializeField] TextMeshProUGUI pressureInfo;
    [SerializeField] TextMeshProUGUI coordinatesInfo;
    [SerializeField] TextMeshProUGUI depthInfo;

    [SerializeField] TextMeshProUGUI fuelLeftInfo;

    [Header("Material infos:")]
    [SerializeField] Material buttonFGMat;
    [SerializeField] Material buttonBGMat;
    [SerializeField] Material buttonHighlightMat;
    [Header("Engine On related:")]
    [SerializeField] TextMeshProUGUI engineStatusInfo;
    [SerializeField] Image engineButtonImage;
    [SerializeField] Image engineIconImage;

    [Header("Sonar modes related:")]
    [SerializeField] TextMeshProUGUI sonarStatusInfo;
    [SerializeField] Image sonarListenButtonImage;
    [SerializeField] Image sonarListenButtonIcon;
    [SerializeField] Image sonarActiveButtonImage;
    [SerializeField] Image sonarActiveButtonIcon;

    [Header("PauseMenu:")]
    [SerializeField] RectTransform pauseMenuRect;
    [SerializeField] Sprite normalIcon;
    [SerializeField] Sprite activeIcon;
    [SerializeField] Image targetImage;
    [SerializeField] float animationDuration = 0.15f;
    bool isPaused = false;
    [SerializeField] AudioMixer globalMixer;
    [SerializeField] Vector2 minMaxDbRange = new Vector2(-40, 20);
    [SerializeField] Slider volumeSlider;

    StatsFaker playerStats;
    SubmarineController player;
    ActiveSonar sonar;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SubmarineController>();
        sonar = GameObject.FindGameObjectWithTag("Player").GetComponent<ActiveSonar>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsFaker>();
        playerStats.OnFakeStatsChanged += UpdateFakeStats;
        playerStats.OnSpeedChanged += UpdateSpeed;

        volumeSlider.onValueChanged.AddListener(AdjustGlobalAudioScale);
    }

    private void UpdateSpeed(string speedText)
    {
        speedInfo.text = speedText;
    }

    private void UpdateFakeStats(string temperature, string pressure, string depth, string coords)
    {
        temperatureInfo.text = temperature;
        pressureInfo.text = pressure;
        depthInfo.text = depth;
        coordinatesInfo.text = coords;
    }


    private void FixedUpdate()
    {
        timeInfo.text = DateTime.Now.ToString();
        energyInfo.text = $"Energy:{player.energy:0.0}%";
    }

    private void OnDisable()
    {
        if(playerStats != null) { 
            playerStats.OnFakeStatsChanged -= UpdateFakeStats; 
            playerStats.OnSpeedChanged -= UpdateSpeed; 
        }

        pauseMenuRect.DOKill();
    }

    public void TogglePauseMenu()
    {
        AudioManagerF.Instance.PlayButtonPressedSound();
        if(!isPaused)
        {
            targetImage.sprite = activeIcon;
            pauseMenuRect.DOScaleY(1f, animationDuration);
            isPaused = true;
        }
        else
        {
            targetImage.sprite = normalIcon;
            pauseMenuRect.DOScaleY(0f, animationDuration);
            isPaused = false;
        }
    }

    public void AdjustGlobalAudioScale(float ratio)
    {
        //float dB = Mathf.Log10(Mathf.Clamp(ratio, 0.0001f, 1f)) * 20f;
        float dB = Mathf.Lerp(minMaxDbRange.x, minMaxDbRange.y, ratio);
        globalMixer.SetFloat("MasterVolume", dB);
    }

    #region EngineButton
    public void ToggleEngine()
    {
        player.ToggleEngine();
        if (player.EngineIsOn)
        {
            engineButtonImage.material = buttonFGMat;
            engineIconImage.material = buttonBGMat;
            engineStatusInfo.text = "On";
            LogEntryController.Instance.AddLogEntryNormal("Turned engines on.");
        }
        else
        {
            engineButtonImage.material = buttonBGMat;
            engineIconImage.material = buttonFGMat;
            engineStatusInfo.text = "Off";
            LogEntryController.Instance.AddLogEntryWarning("Turned engines off!");
        }
    }
    #endregion

    #region SonarButtons
    public void TurnOnSonarActiveMode()
    {
        sonar.TurnOnSonar(false);
        sonarListenButtonIcon.material = buttonFGMat;
        sonarListenButtonImage.material = buttonBGMat;

        sonarActiveButtonIcon.material = buttonHighlightMat;
        sonarActiveButtonImage.material = buttonFGMat;

        sonarStatusInfo.text = "Mode: Active";
        LogEntryController.Instance.AddLogEntryNormal("Switched sonar mode to active.");
    }
    public void TurnOnSonarListenMode()
    {
        sonar.TurnOnSonar(true);
        sonarListenButtonIcon.material = buttonHighlightMat;
        sonarListenButtonImage.material = buttonFGMat;

        sonarActiveButtonIcon.material = buttonFGMat;
        sonarActiveButtonImage.material = buttonBGMat;

        sonarStatusInfo.text = "Mode: Passive";
        LogEntryController.Instance.AddLogEntryNormal("Switched sonar mode to passive.");
    }

    public void UnoverSonarListenButton()
    {
        if (sonar.IsPassiveSonar)
        {
            sonarListenButtonIcon.material = buttonHighlightMat;
            sonarListenButtonImage.material = buttonFGMat;
        }
        else
        {
            sonarListenButtonIcon.material = buttonFGMat;
            sonarListenButtonImage.material = buttonBGMat;
        }
    }
    public void UnoverSonarActiveButton()
    {
        if (!sonar.IsPassiveSonar)
        {
            sonarActiveButtonIcon.material = buttonHighlightMat;
            sonarActiveButtonImage.material = buttonFGMat;
        }
        else
        {
            sonarActiveButtonIcon.material = buttonFGMat;
            sonarActiveButtonImage.material = buttonBGMat;
        }
    }
    #endregion

}
