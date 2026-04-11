using UnityEngine;

public class EntryPoint : MonoBehaviour
{ 
   [SerializeField] private BatteryManager _batteryManager;

    private void Start()
    {
        var battery = _batteryManager.Battery;
    }
}