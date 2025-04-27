using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineSoundsManager : MonoBehaviour
{
    public static SubmarineSoundsManager Instance;

    [Header("Submarine")]
    [SerializeField] private SubmarineSoundEmmiter emmiter;
    [SerializeField] private SubmarineController controller;

    [Header("Sonar")]
    [SerializeField] private AudioSource Sonar;
    [SerializeField] private float SonarSoundRange;

    [Header("Engines")] 
    [SerializeField] private float EngineSoundEmitDelay = 5f;

    [SerializeField] private AudioSource ElectricEngine;
    [SerializeField] private float ElectricEngineSoundRange;
    [SerializeField] private AudioClip ElectricEngineSound;
    [SerializeField] private AudioClip ElectricEngineStartupSound;
    [SerializeField] private AudioClip ElectricEngineOffSound;

    [SerializeField] private AudioSource DieselEngine;
    [SerializeField] private float DieselEngineSoundRange;
    [SerializeField] private AudioClip DieselEngineSound;
    [SerializeField] private AudioClip DieselEngineStartupSound;
    [SerializeField] private AudioClip DieselEngineOffSound;

    [Header("Ambience")]
    [SerializeField] private List<AudioClip> SubAmbienceSounds;
    [SerializeField] private AudioSource SubmarineAmbience;
    [SerializeField] private int minSubSoundBreak;
    [SerializeField] private int maxSubSoundBreak;

    [SerializeField] private AudioSource SubmarineCollision;
    [SerializeField] private List<AudioClip> CollisionSounds;

    private bool EngineIsWorking;

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
    }

    public void EmitSonar()
    {
        Sonar.Play();
        emmiter.SendSound(null, SonarSoundRange);
    }

    private IEnumerator EngineSignalCoroutine(float range)
    {
        while (EngineIsWorking)
        {
            emmiter.SendSound(null, range);
            yield return new WaitForSeconds(EngineSoundEmitDelay);
        }
    }

    public void StartEngine(bool dieselEngineOn)
    {
        ElectricEngine.Stop();
        DieselEngine.Stop();

        if (!dieselEngineOn)
        {
            var coroutine = EngineStartUpCoroutine(ElectricEngineStartupSound, ElectricEngineSound, ElectricEngine);
            StartCoroutine(coroutine);
        }
        else
        {
            var coroutine = EngineStartUpCoroutine(DieselEngineStartupSound, DieselEngineSound, DieselEngine);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator EngineStartUpCoroutine(AudioClip startUp, AudioClip regularSound, AudioSource source)
    {
        source.clip = startUp;
        source.Play();
        
        yield return new WaitForSeconds(startUp.length);
        
        source.Stop();
        source.clip = regularSound;
        source.loop = true;
        source.Play();

        EngineIsWorking = true;

        if (!controller.DieselEngineOn)
        {
            var coroutine = EngineSignalCoroutine(ElectricEngineSoundRange);
            StartCoroutine(coroutine);
        }
        else
        {
            var coroutine = EngineSignalCoroutine(DieselEngineSoundRange);
            StartCoroutine(coroutine);
        }
    }

    public void StopEngines(bool dieselEngineOn)
    {
        EngineIsWorking = false;
        ElectricEngine.Stop();
        DieselEngine.Stop();

        if (!dieselEngineOn)
        {
            ElectricEngine.loop = false;
            ElectricEngine.clip = ElectricEngineOffSound;
            ElectricEngine.Play();
        }
        else
        {
            DieselEngine.loop = false;
            DieselEngine.clip = DieselEngineOffSound;
            DieselEngine.Play();
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
