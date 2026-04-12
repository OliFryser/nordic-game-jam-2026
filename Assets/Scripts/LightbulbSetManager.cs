using System;
using System.Collections.Generic;
using System.Linq;
using Input;
using Modules;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightbulbSetManager : MonoBehaviour
{
    [SerializeField] private LightBulb[] _lightBulbs;
    [SerializeField] private LightBulb[] _computerLightBulbs;
    [SerializeField] private EngineButtonPressEmitter _engineButtonPressEmitter;
    [SerializeField] private float _timeToShowWinLight;

    private LightSet _lightSet;
    private Battery _battery;
    [SerializeField] private float _batteryChargeAmount;
    [SerializeField] private SoundManager _soundManager;

    [SerializeField] private Renderer _cable;
    [SerializeField] private CableManager _cableManager;


    private void Start()
    {
        StartNewSet();
    }

    private void OnEnable()
    {
        _engineButtonPressEmitter.OnPress += OnPress;
        _engineButtonPressEmitter.OnRelease += OnRelease;
    }

    private void OnDisable()
    {
        _engineButtonPressEmitter.OnPress -= OnPress;
        _engineButtonPressEmitter.OnRelease -= OnRelease;
    }

    private void StartNewSet()
    {
        int number = Random.Range(3, 7);

        HashSet<int> set = new();

        while (set.Count < number)
        {
            set.Add(Random.Range(0, 12));
        }

        _lightSet = new LightSet(set.ToArray());

        for (int i = 0; i < _computerLightBulbs.Length; i++)
        {
            if (set.Contains(i))
            {
                _computerLightBulbs[i].TurnOn();
            }
            else
            {
                _computerLightBulbs[i].TurnOff();
            }
        }
    }

    private void OnPress(EngineButton button)
    {
        if (button.Section != DashboardSection.SetLights
            || button.InSectionIndex < 0
            || button.InSectionIndex >= 12)
        {
            return;
        }

        Sound sound = Random.value < .5f ? Sound.Click1 : Sound.Click2;
        _soundManager.Play(sound);
        _lightSet.Press(button.InSectionIndex);
        _lightBulbs[button.InSectionIndex].TurnOn(player: true);
        if (_lightSet.IsComplete())
        {
            OnSetComplete();
        }
    }

    private void OnRelease(EngineButton button)
    {
        if (button.Section != DashboardSection.SetLights)
        {
            return;
        }

        _lightSet.Release(button.InSectionIndex);
        _lightBulbs[button.InSectionIndex].TurnOff();
        if (_lightSet.IsComplete())
        {
            _ = OnSetComplete();
        }
    }

    private async Awaitable OnSetComplete()
    {
        // Light all
        foreach (LightBulb lightBulb in _computerLightBulbs)
        {
            lightBulb.TurnOn(true);
        }

        _battery.AddCharge(_batteryChargeAmount);
        _cableManager.TurnOnCable(_cable, 2f);

        // wait
        await Awaitable.WaitForSecondsAsync(_timeToShowWinLight);

        // new set
        foreach (LightBulb lightBulb in _computerLightBulbs)
        {
            lightBulb.TurnOff();
        }

        await Awaitable.WaitForSecondsAsync(_timeToShowWinLight / 4f);

        StartNewSet();
    }

    public void Initialize(Battery battery)
    {
        _battery = battery;
    }
}

public class LightSet
{
    private readonly bool[] _target;
    private readonly bool[] _playerSet = new bool[12];

    public LightSet(int[] target)
    {
        bool[] targetSet = new bool[12];
        for (int i = 0; i < targetSet.Length; i++)
        {
            if (target.Contains(i))
            {
                targetSet[i] = true;
            }
        }

        _target = targetSet;
    }

    public void Press(int index)
    {
        _playerSet[index] = true;
    }

    public void Release(int index)
    {
        _playerSet[index] = false;
    }

    public bool IsComplete()
    {
        for (int i = 0; i < _playerSet.Length; i++)
        {
            if (_target[i] != _playerSet[i])
            {
                return false;
            }
        }

        return true;
    }
}