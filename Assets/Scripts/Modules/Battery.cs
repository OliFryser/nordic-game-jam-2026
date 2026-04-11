using System;
using UnityEngine;

namespace Modules
{
    [Serializable]
    public class Battery
    {
        public Action<float, float> OnBatteryLevelChanged;
        
        [field: SerializeField]
        public float Charge { get; private set; }

        public void AddCharge(float charge)
        {
            float previousCharge = Charge;
            
            Charge += charge;
            Charge = Mathf.Min(Charge, 1f);
            
            OnBatteryLevelChanged?.Invoke(previousCharge, Charge);
        }
        
        public void RemoveCharge(float charge)
        {
            float previousCharge = Charge;
            
            Charge -= charge;
            Charge = Mathf.Max(Charge, 0f);
            
            OnBatteryLevelChanged?.Invoke(previousCharge, Charge);
        }
    }
}