using System;
using UnityEngine;

namespace Modules
{
    public class Hydraulic : MonoBehaviour
    {
        [SerializeField] private float _costToPump;
        [SerializeField] private float _target;
        [SerializeField] private float _charge;
        [SerializeField] private float _gainOnPump;
        [SerializeField] private float _slack;
        [SerializeField] private Battery _battery;

        [SerializeField] private GameObject _chargeIndicator;
        [SerializeField] private GameObject _targetIndicator;
        [SerializeField] private Transform _top;
        [SerializeField] private Transform _bottom;

        private void Start()
        {
            _chargeIndicator.transform.position = Vector3.Lerp(_bottom.position, _top.position, _charge);
            _targetIndicator.transform.position = Vector3.Lerp(_bottom.position, _top.position, _target);
        }

        public bool IsRegulated => Mathf.Abs(_target - _charge) <= _slack;

        public void Initialize(Battery battery)
        {
            _battery = battery;
        }

        public void TryPump()
        {
            if (_battery.Charge >= _costToPump)
            {
                _battery.RemoveCharge(_costToPump);
                _charge += _gainOnPump;
                _charge = Mathf.Min(_charge, 1f);
            }
        }
    }
}