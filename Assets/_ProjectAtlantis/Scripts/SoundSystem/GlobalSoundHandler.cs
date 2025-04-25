using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalSoundHandler : MonoBehaviour
{
    public static GlobalSoundHandler Instance;
    private List<ISoundSystemMember> members;

    public void Awake()
    {
        members = new List<ISoundSystemMember>();

        if (Instance == null)
            Instance = this;
    }

    public void Register(ISoundSystemMember member)
    {
        members.Add(member);
    }

    public void Send(ISoundSystemMember sender, float distance)
    {
        foreach (var member in members.Where(member => member.TargetSignal == sender.OutputSignal))
        {
            member.OnSignalReceived(sender, distance);
        }
    }
}
