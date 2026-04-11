using System;
using Input;
using Modules;
using UnityEngine;

public class LightBulbManager : MonoBehaviour
{
    [SerializeField] private LightBulb[] _lightBulbs;
    [SerializeField] private float _waitBetweenLightBulbs;
    [SerializeField] private float _waitBetweenSequences;
    [SerializeField] private Sequence _sequence;
    [SerializeField] private EngineButtonPressEmitter _engineButtonPressEmitter;
    
    private bool _isPlaying;

    private void Start()
    {
        Play(_sequence);
        _engineButtonPressEmitter.OnPress += OnPress;
        _engineButtonPressEmitter.OnRelease += OnRelease;
    }
    
    private void OnPress(EngineButton button)
    {
        print(button.Section);
        if (button.Section != DashboardSection.Lights)
        {
            return;
        }
        
        Interrupt();
        TurnOn(button.InSectionIndex);
    }
    
    private void OnRelease(EngineButton button)
    {
        if (button.Section != DashboardSection.Lights)
        {
            return;
        }
        
        TurnOff(button.InSectionIndex);
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
    
            TurnOffAll();
            await Awaitable.WaitForSecondsAsync(_waitBetweenSequences);
        }
    }

    private void Interrupt()
    {
        _isPlaying = false;
    }

    private void TurnOffAll()
    {
        foreach (LightBulb lightBulb in _lightBulbs)
        {
            lightBulb.TurnOff();
        }
    }

    private void TurnOn(int index)
    {
        print(index);
        _lightBulbs[index].TurnOn();
    }

    private void TurnOff(int index)
    {
        _lightBulbs[index].TurnOff();
    }
}