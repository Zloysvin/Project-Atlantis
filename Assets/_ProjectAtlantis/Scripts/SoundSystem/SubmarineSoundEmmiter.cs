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
        var actualDistance = Vector2.Distance(transform.position, sender.Transform.position);
        if (actualDistance <= distance)
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

            sender.Audio.volume = (100f - totalDistance / (distance  * 0.01f)) * 0.01f;

            sender.PlaySound();

            GetDirectionInHours(sender.Transform.position, actualDistance, sender.SoundRange);

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

    private void GetDirectionInHours(Vector3 target, float distance, float maxDistance)
    {
        Vector3 direction = target - transform.position;
        direction = Quaternion.AngleAxis(-90, Vector3.forward) * direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if(angle <= 0)
            angle *= -1;
        else
        {
            angle = 360 - angle;
        }

        float distanceRatio = Mathf.Clamp01(distance / maxDistance);
        float maxError = 60f;
        float error = (1f - distanceRatio) * maxError;
        angle += Random.Range(-error, error);

        int hour = Mathf.RoundToInt(angle / 30f);
        if (hour == 0) hour = 12;
        else if (hour > 12) hour -= 12;

        //Debug.Log($"Contact at {hour} o'clock. Distance {(int)distance}. Actual angle {angle}");
    }
}
