using System;
using System.Linq;
using Input;
using Modules;
using UnityEngine;

public class HydraulicsManager : MonoBehaviour
{
    [SerializeField] private float _costToPump;
    [SerializeField] private float _gainOnPump;
    [SerializeField] private float _dropAmount;
    [SerializeField] private float _slack;
    [SerializeField] private Battery _battery;

    [SerializeField] private EngineButtonPressEmitter _engineButtonPressEmitter;
    [SerializeField] private Hydraulic[] _hydraulics;

    [SerializeField] private LightbulbMaterialController _lightbulbCorrectConfiguration;
    
    private void Start()
    {
        foreach (Hydraulic hydraulic in _hydraulics)
        {
            hydraulic.Initialize(_slack);
        }
    }

    private void OnEnable()
    {
        _engineButtonPressEmitter.OnPress += OnPress;
    }

    private void OnDisable()
    {
        _engineButtonPressEmitter.OnPress -= OnPress;
    }

    public void Initialize(Battery battery)
    {
        _battery = battery;
    }

    private void Update()
    {
        foreach (Hydraulic hydraulic in _hydraulics)
        {
            hydraulic.Drop(_dropAmount);
        }

        foreach (Hydraulic hydraulic in _hydraulics)
        {
            bool on = hydraulic.IsRegulated(_slack);
            hydraulic.Set(on);
        }
        
        _lightbulbCorrectConfiguration.Set(_hydraulics.All(h => h.IsRegulated(_slack)));
    }

    private void OnPress(EngineButton button)
    {
        print($"{button.InSectionIndex} pressed from {button.Section}");

        if (button.Section != DashboardSection.Hydraulics
            || button.InSectionIndex < 0
            || button.InSectionIndex >= 12)
        {
            return;
        }

        if (_battery.Charge >= _costToPump)
        {
            _battery.RemoveCharge(_costToPump);
            _hydraulics[button.InSectionIndex].Pump(_gainOnPump);
        }
    }
}