using System;
using System.Linq;
using Input;
using Modules;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightBulbManager : MonoBehaviour
{
    [SerializeField] private LightBulb[] _lightBulbs;
    [SerializeField] private LightBulb[] _computerLightBulbs;
    [SerializeField] private float _waitBetweenLightBulbs;
    [SerializeField] private float _waitBetweenSequences;
    [SerializeField] private Sequence _sequence;
    [SerializeField] private EngineButtonPressEmitter _engineButtonPressEmitter;
    [SerializeField] private SequenceOnCompleteEmitter _sequenceOnCompleteEmitter;
    
    private bool _isPlaying;

    private void Start()
    {
        StartNewSequence();
    }

    private void OnEnable()
    {
        _engineButtonPressEmitter.OnPress += OnPress;
        _engineButtonPressEmitter.OnRelease += OnRelease;
        _sequenceOnCompleteEmitter.OnSequenceComplete += OnSequenceComplete;
    }

    private void OnDisable()
    {
        _engineButtonPressEmitter.OnPress -= OnPress;
        _engineButtonPressEmitter.OnRelease -= OnRelease;
        _sequenceOnCompleteEmitter.OnSequenceComplete -= OnSequenceComplete;
    }

    private void StartNewSequence()
    {
        _sequence = 
            new Sequence(Enumerable.Range(0, 3).Select(_ => Random.Range(0, 12)).ToArray());
        // _sequence.AddOnSequenceCompleteListener(_sequenceOnCompleteEmitter.OnSequenceComplete);
        _sequence.OnComplete += OnSequenceComplete;
        Play(_sequence);
    }
    
    private void OnPress(EngineButton button)
    {
        if (button.Section != DashboardSection.Lights 
            || button.InSectionIndex < 0)
        {
            return;
        }
        
        _sequence.Enter(button.InSectionIndex);
        TurnOn(button.InSectionIndex, true);
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
            foreach (int i in _sequence.Numbers)
            {
                for (int j = 0; j < _computerLightBulbs.Length; j++)
                {
                    if (!_isPlaying) return;
                    if (i == j) _computerLightBulbs[j].TurnOn();
                    else _computerLightBulbs[j].TurnOff();
                }

                await Awaitable.WaitForSecondsAsync(_waitBetweenLightBulbs);

                if (_isPlaying)
                {
                    for (int j = 0; j < _computerLightBulbs.Length; j++)
                    {
                        _computerLightBulbs[j].TurnOff();
                    }
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

    private void OnSequenceComplete()
    {
        OnSequenceCompleteAsync();
    }
    
    private async Awaitable OnSequenceCompleteAsync()
    {
        Interrupt();
        
        foreach (LightBulb lightBulb in _computerLightBulbs)
        {
            lightBulb.TurnOn(true);
        }
        
        await Awaitable.WaitForSecondsAsync(_waitBetweenSequences);
        
        TurnOffAll();
        StartNewSequence();
    }
    
    private void TurnOffAll()
    {
        foreach (LightBulb lightBulb in _lightBulbs)
        {
            lightBulb.TurnOff();
        }
    }

    private void TurnOn(int index, bool isPlayer)
    {
        int i = index % 12;
        _lightBulbs[i].TurnOn(isPlayer);
    }

    private void TurnOff(int index)
    {
        int i = index % 12;
        _lightBulbs[i].TurnOff();
    }
}