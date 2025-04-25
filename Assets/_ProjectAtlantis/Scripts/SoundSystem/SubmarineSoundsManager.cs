using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineSoundsManager : MonoBehaviour
{
    public static SubmarineSoundsManager Instance;

    [SerializeField] private SubmarineSoundEmmiter emmiter;
    [SerializeField] private AudioSource Sonar;
    [SerializeField] private float SonarSoundRange;

    [SerializeField] private AudioSource ElectricEngine;
    [SerializeField] private float ElectricEngineSoundRange;
    [SerializeField] private AudioSource DieselEngine;
    [SerializeField] private float DieselEngineSoundRange;

    [SerializeField] private List<AudioClip> SubAmbienceSounds;
    [SerializeField] private AudioSource SubmarineAmbience;
    [SerializeField] private int minSubSoundBreak;
    [SerializeField] private int maxSubSoundBreak;

    [SerializeField] private AudioSource SubmarineCollision;
    [SerializeField] private List<AudioClip> CollisionSounds;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        StartCoroutine("SubAmbienceSound");
    }

    void Update()
    {
        EmitElectricEngine(); // TODO: Add method to toggle Electric and Diesel Engines
    }

    public void EmitSonar()
    {
        Sonar.Play();
        emmiter.SendSound(null, SonarSoundRange);
    }

    public void EmitElectricEngine()
    {
        if(!ElectricEngine.isPlaying)
        {
            ElectricEngine.Play();
            emmiter.SendSound(null, ElectricEngineSoundRange);
        }
    }

    public void EmitDieselEngine()
    {
        if(!DieselEngine.isPlaying)
        {
            DieselEngine.Play();
            emmiter.SendSound(null, DieselEngineSoundRange);
        }
    }

    public void EmitCollision(float collisionRange)
    {
        AudioClip CollisionSound = CollisionSounds[Random.Range(0, CollisionSounds.Count)];
        SubmarineCollision.clip = CollisionSound;
        SubmarineCollision.Play();
        emmiter.SendSound(null, collisionRange);
    }

    //AMBIENCE

    public void SubAmbience()
    {
        AudioClip subSound = SubAmbienceSounds[Random.Range(0, SubAmbienceSounds.Count)];
        SubmarineAmbience.clip = subSound;
        SubmarineAmbience.Play();
    }

    private IEnumerator SubAmbienceSound()
    {
        while (true)
        {
            SubAmbience();
            yield return new WaitForSeconds(Random.Range(minSubSoundBreak, maxSubSoundBreak));
        }
    }
}
