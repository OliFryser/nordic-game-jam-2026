using System;
using Modules;
using UnityEngine;

public class LightBulbManager : MonoBehaviour
{
    [SerializeField] private LightBulb[] _lightBulbs;
    [SerializeField] private float _waitBetweenLightBulbs;
    [SerializeField] private float _waitBetweenSequences;
    [SerializeField] private Sequence _sequence;
    
    private bool _isPlaying;

    private void Start()
    {
        Play(_sequence);
    }

    public void Play(Sequence sequence)
    {
        _sequence = sequence;
        _isPlaying = true;
        Play();
    }

    private async Awaitable Play()
    {
        while (_isPlaying)
        {
            foreach (int i in _sequence.RelativeSequenceNumbers)
            {
                for (int j = 0; j < _lightBulbs.Length; j++)
                {
                    if (!_isPlaying) return;
                    if (i == j) _lightBulbs[j].TurnOn();
                    else _lightBulbs[j].TurnOff();
                }

                await Awaitable.WaitForSecondsAsync(_waitBetweenLightBulbs);
            }

            await Awaitable.WaitForSecondsAsync(_waitBetweenSequences);
        }
    }

    public void Interrupt()
    {
        _isPlaying = false;
        foreach (LightBulb lightBulb in _lightBulbs)
        {
            lightBulb.TurnOff();
        }
    }
}