using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuMonsterAI : MonoBehaviour
{
    [SerializeField] RotateToTarget[] monster;
    [SerializeField] Transform[] targetPoint;
    [SerializeField] float maxTravelDistance = 6f;

    [Header("Ditry sound:")]
    [SerializeField] AudioMixer globalMixer;
    [SerializeField] Vector2 minMaxDbRange = new Vector2(-40, 20);
    [SerializeField] Slider volumeSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volumeSlider.onValueChanged.AddListener(AdjustGlobalAudioScale);
        for (int i = 0; i < monster.Length; i++)
        {
            targetPoint[i].position = Random.insideUnitCircle * maxTravelDistance;
            monster[i].target = targetPoint[i];
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < monster.Length; i++)
        {
            if (Vector2.SqrMagnitude(monster[i].transform.position - targetPoint[i].position) <= 0.1f)
                targetPoint[i].position = Random.insideUnitCircle * maxTravelDistance;
        }
    }

    public void AdjustGlobalAudioScale(float ratio)
    {
        //float dB = Mathf.Log10(Mathf.Clamp(ratio, 0.0001f, 1f)) * 20f;
        float dB = Mathf.Lerp(minMaxDbRange.x, minMaxDbRange.y, ratio);
        globalMixer.SetFloat("MasterVolume", dB);
    }
}
