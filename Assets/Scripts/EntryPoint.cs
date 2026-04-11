using UnityEngine;

public class EntryPoint : MonoBehaviour
{ 
   [SerializeField] private BatteryManager _batteryManager;
   [SerializeField] private HydraulicsManager _hydraulicsManager;
   
    private void Start()
    {
        var battery = _batteryManager.Battery;
        _hydraulicsManager.Initialize(battery);
    }
}