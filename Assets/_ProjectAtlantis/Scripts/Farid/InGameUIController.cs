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

    private void Awake()
    {
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
        Debug.Log(dB);
        globalMixer.SetFloat("MasterVolume", dB);
    }

    
    
}
