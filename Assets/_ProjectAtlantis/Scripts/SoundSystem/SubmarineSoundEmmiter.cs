using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SubmarineSoundEmmiter : MonoBehaviour, ISoundSystemMember
{
    [field: SerializeField] public SignalType TargetSignal { get; set; }
    [field: SerializeField] public SignalType OutputSignal { get; set; }
    [field: SerializeField] public float SoundRange { get; set; }
    [field: SerializeField] public AudioSource Audio { get; set; }

    public Transform Transform { get; set; }

    private ActiveSonar sonar;

    public void Awake()
    {
        Audio = GetComponent<AudioSource>();

        Transform = transform;
    }

    public void Start()
    {
        GlobalSoundHandler.Instance.Register(this);
        sonar = GetComponent<ActiveSonar>();
    }

    public void OnSignalReceived(ISoundSystemMember sender, float distance)
    {
        if (Vector2.Distance(transform.position, sender.Transform.position) <= distance)
        {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, sender.Transform.position, NavMesh.AllAreas, path);

            float totalDistance = 0f;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.blue, 5f);
                totalDistance += Vector2.Distance(path.corners[i], path.corners[i + 1]);
            }

            if (totalDistance > distance)
                return;

            if (path.corners.Length == 0)
                totalDistance = Vector2.Distance(transform.position, sender.Transform.position);

            sender.Audio.volume = (100f - totalDistance / (distance / 100f))/100f;

            sender.PlaySound();

            if (!sonar.IsPassiveSonar)
                return;

            PingDisplayHandler.Instance.DisplayPing(sender.Transform.position);

            int pingAmounts = Random.Range(3, 7);
            for (int j = 0; j < pingAmounts; j++)
            {
                PingDisplayHandler.Instance.DisplayPing(sender.Transform.position +
                                                        new Vector3(Random.Range(0.1f, 1f),
                                                            Random.Range(0.1f, 1f)));
            }
        }
    }

    public void SendSound(AudioClip SFX, float soundRange)
    {
        SoundRange = soundRange;
        GlobalSoundHandler.Instance.Send(this, SoundRange);
    }

    public void PlaySound()
    {
        Audio.Play();
    }
}
