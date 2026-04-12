using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _click1;
    [SerializeField] private AudioClip _click2;
    
    
    public void Play(Sound sound)
    {
        AudioClip clip = sound switch
        {
            Sound.Click1 => _click1,
            Sound.Click2 => _click2,
            _ => throw new ArgumentOutOfRangeException(nameof(sound), sound, null)
        };
        
        print("Playing " + clip);
        _audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.PlayOneShot(clip);
    }
}

public enum Sound
{
    Click1,
    Click2
}