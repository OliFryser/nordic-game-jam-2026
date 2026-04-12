using Modules;
using Modules.Conveyor;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{ 
   [SerializeField] private BatteryManager _batteryManager;
   [SerializeField] private HydraulicsManager _hydraulicsManager;
   [SerializeField] private LightBulbManager _lightBulbManager;
   [SerializeField] private LightbulbSetManager _lightbulbSetManager;
   [SerializeField] private ConveyorManager _conveyorManager;
   [SerializeField] private VFXManager _vfxManager;
   
    private void Start()
    {
        Battery battery = new Battery();
        _batteryManager.Initialize(battery);
        _hydraulicsManager.Initialize(battery);
        _lightBulbManager.Initialize(battery);
        _lightbulbSetManager.Initialize(battery);
        _conveyorManager.Initialize(battery);
        _vfxManager.Initialize(battery);
    }
}