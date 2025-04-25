using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSoundEmmiter : MonoBehaviour, ISoundSystemMember
{
    [field: SerializeField] public SignalType TargetSignal { get; set; }
    [field: SerializeField] public SignalType OutputSignal { get; set; }
    [field: SerializeField] public float SoundRange { get; set; }
    [field: SerializeField] public AudioSource Audio { get; set; }

    public Transform Transform { get; set; }

    private AIController controller;

    public void Awake()
    {
        Transform = transform;

        Audio = GetComponent<AudioSource>();
        controller = GetComponent<AIController>();
    }

    public void Start()
    {
        GlobalSoundHandler.Instance.Register(this);
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
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 5f);
                totalDistance += Vector2.Distance(path.corners[i], path.corners[i + 1]);
            }

            if(totalDistance > distance)
                return;

            controller.CheckAgro(sender.Transform.position, totalDistance);
        }
    }

    public void SendSound(AudioClip SFX, float soundRange)
    {
        Audio.clip = SFX;
        SoundRange = soundRange;
        GlobalSoundHandler.Instance.Send(this, SoundRange);
    }

    public void PlaySound()
    {
        Audio.Play();
    }
}
