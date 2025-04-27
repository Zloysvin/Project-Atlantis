using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class StatsFaker : MonoBehaviour
{
    public static StatsFaker Instance;
    [SerializeField] float updateInterval = 0.6f;
    [SerializeField] Vector2 verticalLevelDimension;
    [SerializeField] Vector2 horizontalLevelDimension;

    [SerializeField] AnimationCurve temperatureDistributionCurve;
    [SerializeField] AnimationCurve meterDistributionCurve;
    [SerializeField] Material depthMeterMaterial;
    [SerializeField] float meterChangeSpeed = 1f;
    [SerializeField] float scrollActivationThreshold = 0.3f;
    [SerializeField] AnimationCurve northCoordCurve;
    [SerializeField] AnimationCurve eastCoordCurve;

    [SerializeField] float playerMaxVelocity = 0.6f;
    [SerializeField] AnimationCurve speedCurve;
    Rigidbody2D rb;

    float currentRatioX;
    float currentRatioY;
    public string CurrentTemperature;
    public string CurrentPressure;
    public string CurrentDepth;
    public string CurrentCords;
    public Action<string, string, string, string> OnFakeStatsChanged;

    public string CurrentSpeed;
    public Action<string> OnSpeedChanged;

    public float currentSpeedRatio;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
            Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMaxVelocity *= playerMaxVelocity;
        StartCoroutine(EvaluateStats());
    }

    private void FixedUpdate()
    {
        float velocityRatio = rb.linearVelocity.sqrMagnitude / playerMaxVelocity;
        OnSpeedChanged?.Invoke($"Speed: {speedCurve.Evaluate(velocityRatio):0.00f}kn");

        //currentSpeedRatio = rb.linearVelocityY / playerMaxVelocity;
        //if (Mathf.Abs(currentSpeedRatio) <= 0.3f) currentSpeedRatio = 0f;
        //currentSpeedRatio *= -1;
        if(rb.linearVelocityY >= scrollActivationThreshold)
            depthMeterMaterial.SetFloat("_TextureScrollYSpeed", meterChangeSpeed);
        else if(rb.linearVelocityY <= -scrollActivationThreshold)
            depthMeterMaterial.SetFloat("_TextureScrollYSpeed", -meterChangeSpeed);
        else
            depthMeterMaterial.SetFloat("_TextureScrollYSpeed", 0f);
    }

    IEnumerator EvaluateStats()
    {
        while (true)
        {
            currentRatioX = (transform.position.x - horizontalLevelDimension.x) / horizontalLevelDimension.y;
            currentRatioY = (transform.position.y - verticalLevelDimension.x) / verticalLevelDimension.y;

            if(PingDisplayHandler.Instance.CrazynessFactor > 0.2f)
            {
                float factor = PingDisplayHandler.Instance.CrazynessFactor;

                CurrentTemperature =
                    $"Temp: {(temperatureDistributionCurve.Evaluate(currentRatioY) + Random.Range(-factor, factor)):0.0}°";
                CurrentPressure = $"{(meterDistributionCurve.Evaluate(currentRatioY) * 0.1 + 1 + Random.Range(-factor, factor) * 10f):0}bar";
                CurrentDepth = $"{(meterDistributionCurve.Evaluate(currentRatioY) + Random.Range(-factor, factor) * 300f):0} meters";
                CurrentCords =
                    $"{(northCoordCurve.Evaluate(currentRatioY) + Random.Range(-factor, factor) * 7):0.0}°N,{(eastCoordCurve.Evaluate(currentRatioX) + Random.Range(-factor, factor) * 9):0.0}°E";
            }
            else
            {
                CurrentTemperature =
                    $"Temp: {(temperatureDistributionCurve.Evaluate(currentRatioY)):0.0}°";
                CurrentPressure = $"{(meterDistributionCurve.Evaluate(currentRatioY) * 0.1 + 1):0}bar";
                CurrentDepth = $"{(meterDistributionCurve.Evaluate(currentRatioY)):0} meters";
                CurrentCords =
                $"{(northCoordCurve.Evaluate(currentRatioY)):0.0}°N,{(eastCoordCurve.Evaluate(currentRatioX)):0.0}°E";
            }

            OnFakeStatsChanged?.Invoke(CurrentTemperature, CurrentPressure, CurrentDepth, CurrentCords);
            yield return new WaitForSeconds(updateInterval);
        }
    }
    public string GetCurrentCoordinates()
    {
        return $"{northCoordCurve.Evaluate(currentRatioY):0.0}°N,{eastCoordCurve.Evaluate(currentRatioX):0.0}°E";
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
