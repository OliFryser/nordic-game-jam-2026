using System;
using System.Linq;
using System.Threading;
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
    private CancellationTokenSource _cts;
    private bool _isPlaying;
    private Battery _battery;
    [SerializeField] private float _batterChargeAmount;
    [SerializeField] private SoundManager _soundManager;
    
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
        _cts?.Cancel();
        _cts?.Dispose();
    }

    private void StartNewSequence()
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        _sequence = 
            new Sequence(Enumerable.Range(0, 3).Select(_ => Random.Range(0, 12)).ToArray());
        // _sequence.AddOnSequenceCompleteListener(_sequenceOnCompleteEmitter.OnSequenceComplete);
        _sequence.OnComplete += OnSequenceComplete;
        Play(_sequence, _cts.Token);
    }
    
    private void OnPress(EngineButton button)
    {
        if (button.Section != DashboardSection.Lights 
            || button.InSectionIndex < 0
            || button.InSectionIndex >= 12)
        {
            return;
        }
        
        Sound sound = Random.value < .5f ? Sound.Click1 : Sound.Click2;
        _soundManager.Play(sound);
        _sequence.Enter(button.InSectionIndex);
        TurnOn(button.InSectionIndex, true);
    }
    
    private void OnRelease(EngineButton button)
    {
        if (button.Section != DashboardSection.Lights
            || button.InSectionIndex < 0
            || button.InSectionIndex >= 12)
        {
            return;
        }
        
        TurnOff(button.InSectionIndex);
    }


    public void Play(Sequence sequence, CancellationToken token)
    {
        _sequence = sequence;
        _isPlaying = true;
        _ = Play(token);
    }

    private async Awaitable Play(CancellationToken token)
    {
        while (_isPlaying && !token.IsCancellationRequested)
        {
            foreach (int i in _sequence.Numbers)
            {
                for (int j = 0; j < _computerLightBulbs.Length; j++)
                {
                    if (!_isPlaying) return;
                    if (i == j) _computerLightBulbs[j].TurnOn();
                    else _computerLightBulbs[j].TurnOff();
                }

                await Awaitable.WaitForSecondsAsync(_waitBetweenLightBulbs, token);

                if (_isPlaying)
                {
                    for (int j = 0; j < _computerLightBulbs.Length; j++)
                    {
                        _computerLightBulbs[j].TurnOff();
                    }
                }
                
                await Awaitable.WaitForSecondsAsync(_waitBetweenLightBulbs, token);
            }
    
            TurnOffAll();
            await Awaitable.WaitForSecondsAsync(_waitBetweenSequences, token);
        }
    }

    private void Interrupt()
    {
        _isPlaying = false;
    }

    private void OnSequenceComplete()
    {
        _ = OnSequenceCompleteAsync();
    }
    
    private async Awaitable OnSequenceCompleteAsync()
    {
        Interrupt();
        
        foreach (LightBulb lightBulb in _computerLightBulbs)
        {
            lightBulb.TurnOn(true);
        }
        
        _battery.AddCharge(_batterChargeAmount);
        
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

    public void Initialize(Battery battery)
    {
        _battery = battery;
    }
}