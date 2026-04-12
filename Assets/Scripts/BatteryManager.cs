using System;
using LitMotion;
using LitMotion.Extensions;
using Modules;
using NaughtyAttributes;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    [SerializeField] private SequenceOnCompleteEmitter _sequenceOnCompleteEmitter;
    [SerializeField] private Transform _batteryLevelVisual;
    [SerializeField] private Material _batteryLevelMaterial;
    [SerializeField] private Color _batteryEmptyColor;
    [SerializeField] private Color _batteryFullColor;

    [SerializeField] private float _batteryDrainRate;
    [SerializeField] private Lever _lever;
    
    public Battery Battery { get; private set; }

    private bool ShouldDrainBattery { get; set; } = true;
    private float ChargeAmount => 1f / 6f;

    public void Initialize(Battery battery)
    {
        Battery = battery;
        Battery.OnBatteryLevelChanged += OnBatteryLevelChanged;
    }
    
    private void Awake()
    {
        _batteryLevelMaterial.SetColor("_BaseColor", _batteryEmptyColor);
    }

    private void Update()
    {
        if (ShouldDrainBattery)
        {
            Battery.RemoveCharge(_batteryDrainRate * Time.deltaTime);
        }
    }


    private void OnEnable()
    {
        _sequenceOnCompleteEmitter.OnSequenceComplete += OnSequenceComplete;
    }
    
    private void OnBatteryLevelChanged(float previousCharge, float currentCharge)
    {
        LMotion.Create(previousCharge, currentCharge, 0.2f)
            .WithEase(Ease.InOutQuad)
            .BindToLocalScaleY(_batteryLevelVisual);
        Color previousColor = _batteryLevelMaterial.GetColor("_BaseColor");
        Color newColor = Color.Lerp(_batteryEmptyColor, _batteryFullColor, currentCharge);
        LMotion.Create(previousColor, newColor, 0.2f)
            .WithEase(Ease.InOutQuad)
            .Bind((c) => _batteryLevelMaterial.SetColor("_BaseColor", c));

        if (Battery.Charge > 0.99f)
        {
            _lever.Push();
            ShouldDrainBattery = false;
        }
    }

    private void OnDisable()
    {
        _sequenceOnCompleteEmitter.OnSequenceComplete -= OnSequenceComplete;
        Battery.OnBatteryLevelChanged -= OnBatteryLevelChanged;

    }

    [Button]
    private void OnSequenceComplete()
    {
        Battery.AddCharge(ChargeAmount);
    }
}