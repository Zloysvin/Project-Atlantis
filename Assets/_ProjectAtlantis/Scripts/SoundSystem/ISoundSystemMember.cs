using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public enum SignalType
{
    Monster, Sub
}

public interface ISoundSystemMember
{
    public SignalType TargetSignal { get; set; }
    public SignalType OutputSignal { get; set; }
    public float SoundRange { get; set; }
    public AudioSource Audio { get; set; }
    public Transform Transform { get; set; }

    public void OnSignalReceived(ISoundSystemMember sender, float distance);

    public void SendSound(AudioClip SFX, float soundRange);

    public void PlaySound();
}
