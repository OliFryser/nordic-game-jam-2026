using Modules;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    [SerializeField] 
    private SequenceOnCompleteEmitter _sequenceOnCompleteEmitter;

    [SerializeField]
    private Battery _battery;
        
    private void OnEnable()
    {
        _sequenceOnCompleteEmitter.OnSequenceComplete += OnSequenceComplete;
    }

    private void OnDisable()
    {
        _sequenceOnCompleteEmitter.OnSequenceComplete -= OnSequenceComplete;
    }

    private void OnSequenceComplete()
    {
        _battery.AddCharge(0.167f);
    }
}