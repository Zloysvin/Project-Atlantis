using System;
using TMPro;
using UnityEngine;

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
    [SerializeField] TextMeshProUGUI beaconsLeftInfo;
    [SerializeField] TextMeshProUGUI consoleInfo;


    StatsFaker playerStats;

    private void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsFaker>();
        playerStats.OnFakeStatsChanged += UpdateFakeStats;
        playerStats.OnSpeedChanged += UpdateSpeed;
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
    }
}
