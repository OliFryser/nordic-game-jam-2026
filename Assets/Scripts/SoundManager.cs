using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _click1;
    [SerializeField] private AudioClip _click2;
    [SerializeField] private AudioClip _conveyorCrane;
    [SerializeField] private AudioClip _alarm;
    
    
    public void Play(Sound sound)
    {
        AudioClip clip = sound switch
        {
            Sound.Click1 => _click1,
            Sound.Click2 => _click2,
            Sound.ConveyorCrane => _conveyorCrane,
            Sound.Alarm => _alarm,
            _ => throw new ArgumentOutOfRangeException(nameof(sound), sound, null)
        };
        
        _audioSource.volume = Sound.ConveyorCrane == sound ? .5f : 1f;
        _audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.PlayOneShot(clip);
    }
}

public enum Sound
{
    Click1,
    Click2,
    ConveyorCrane,
    Alarm
}