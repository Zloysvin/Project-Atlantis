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

    DateTime currentTime = DateTime.Now;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        energyInfo.text = currentTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        energyInfo.text = DateTime.Now.ToString();
    }
}
