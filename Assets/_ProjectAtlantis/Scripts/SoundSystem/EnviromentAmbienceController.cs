using UnityEngine;

public class EnviromentAmbienceController : MonoBehaviour
{
    public static EnviromentAmbienceController Instance;

    [SerializeField] private AudioSource baseAmbience;
    [SerializeField] private float ambienceVolumeMax;
    [SerializeField] private float ambienceVolumeMin;
    [SerializeField] private AudioSource artifactAmbience;
    [SerializeField] private float artifactVolumeMax;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void ArtifactDistanceEffect(float distanceRatio)
    {
        baseAmbience.volume = ambienceVolumeMax - (ambienceVolumeMax - ambienceVolumeMin) * distanceRatio;

        artifactAmbience.volume = artifactVolumeMax * (1 - distanceRatio);
    }

    public void NormalizeSoundVolume()
    {
        artifactAmbience.volume = 0f;
        baseAmbience.volume = ambienceVolumeMax;
    }
}
