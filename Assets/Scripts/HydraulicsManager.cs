using System;
using System.Linq;
using Input;
using Modules;
using UnityEngine;
using Random = UnityEngine.Random;

public class HydraulicsManager : MonoBehaviour
{
    [SerializeField] private float _costToPump;
    [SerializeField] private float _gainOnPump;
    [SerializeField] private float _dropAmount;
    [SerializeField] private float _slack;
    [SerializeField] private Battery _battery;

    [SerializeField] private EngineButtonPressEmitter _engineButtonPressEmitter;
    [SerializeField] private Hydraulic[] _hydraulics;
    [SerializeField] private bool[] _isPressed;
    private float[] _dropAmounts;

    [SerializeField] private float _batteryChargeAmount;

    [SerializeField] private LightbulbMaterialController _lightbulbCorrectConfiguration;
    [SerializeField] private DashboardSection _dashboardSection;

    [SerializeField] private Renderer _cableLeft;
    [SerializeField] private Renderer _cableRight;
    [SerializeField] private CableManager _cableManager;

    
    private void Start()
    {
        _isPressed = new bool[_hydraulics.Length];
        _dropAmounts = new float[_hydraulics.Length];
        for (int i = 0; i < _dropAmounts.Length; i++)
        {
            _dropAmounts[i] = _dropAmount + Random.Range(-_dropAmount * 0.4f, _dropAmount * 0.4f);
        }
        
        foreach (Hydraulic hydraulic in _hydraulics)
        {
            hydraulic.Initialize(_slack);
        }
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

    public void Initialize(Battery battery)
    {
        _battery = battery;
    }

    private void Update()
    {
        for (int i = 0; i < _hydraulics.Length; i++)
        {
            if (_isPressed[i] && _battery.Charge >= _costToPump)
            {
                _battery.RemoveCharge(_costToPump);
                _hydraulics[i].Pump(_gainOnPump * Time.deltaTime);
            }
            else
            {
                _hydraulics[i].Drop(_dropAmounts[i] * Time.deltaTime);
            }
        }

        foreach (Hydraulic hydraulic in _hydraulics)
        {
            bool on = hydraulic.IsRegulated(_slack);
            hydraulic.Set(on);
        }

        foreach (Hydraulic hydraulic in _hydraulics.Where(h => h.IsRegulated(_slack)))
        {
            _battery.AddCharge(Time.deltaTime * _batteryChargeAmount);
        }
        
        _lightbulbCorrectConfiguration.Set(_hydraulics.All(h => h.IsRegulated(_slack)));
    }

    private void OnPress(EngineButton button)
    {
        if (button.Section != DashboardSection.HydraulicsLeft
            && button.Section != DashboardSection.HydraulicsRight
            || button.InSectionIndex < 0
            || button.InSectionIndex >= 12)
        {
            return;
        }

        if (_battery.Charge >= _costToPump)
        {
            int offset = button.Section == DashboardSection.HydraulicsLeft ? 0 : 12;
            _isPressed[button.InSectionIndex + offset] = true;
        }
    }
    
    private void OnRelease(EngineButton button)
    {
        if (button.Section != DashboardSection.HydraulicsLeft
            && button.Section != DashboardSection.HydraulicsRight
            || button.InSectionIndex < 0
            || button.InSectionIndex >= 12)
        {
            return;
        }
        
        int offset = button.Section == DashboardSection.HydraulicsLeft ? 0 : 12;
        _isPressed[button.InSectionIndex + offset] = false;
    }
}