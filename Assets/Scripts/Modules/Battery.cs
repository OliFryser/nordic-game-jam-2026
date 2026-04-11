using System;
using UnityEngine;

namespace Modules
{
    [Serializable]
    public class Battery
    {
        [field: SerializeField]
        private float Charge { get; set; }

        public void AddCharge(float charge)
        {
            Charge += charge;
            Charge = Mathf.Min(Charge, 1f);
        }
        
        public void RemoveCharge(float charge)
        {
            Charge -= charge;
            Charge = Mathf.Max(Charge, 0f);
        }
    }
}