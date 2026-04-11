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
    public Battery Battery { get; private set; }
    
    private float ChargeAmount => 1f / 6f;

    private void Awake()
    {
        Battery = new Battery();
    }

    private void OnEnable()
    {
        Battery.OnBatteryLevelChanged += OnBatteryLevelChanged;
        _sequenceOnCompleteEmitter.OnSequenceComplete += OnSequenceComplete;
    }

    private void OnBatteryLevelChanged(float previousCharge, float currentCharge)
    {
        LMotion.Create(previousCharge, currentCharge, 0.2f)
            .WithEase(Ease.InOutQuad)
            .BindToLocalScaleY(_batteryLevelVisual);
    }

    private void OnDisable()
    {
        _sequenceOnCompleteEmitter.OnSequenceComplete -= OnSequenceComplete;
        Battery.OnBatteryLevelChanged -= OnBatteryLevelChanged;

    }

    [Button("Add Charge")]
    private void OnSequenceComplete()
    {
        Battery.AddCharge(ChargeAmount);
    }
}