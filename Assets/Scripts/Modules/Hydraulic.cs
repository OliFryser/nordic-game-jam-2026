using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Modules
{
    public class Hydraulic : MonoBehaviour
    {
        [SerializeField] private float _target;
        [SerializeField] private float _charge;

        [SerializeField] private GameObject _chargeIndicator;
        [SerializeField] private GameObject _lowerIndicator;
        [SerializeField] private GameObject _upperIndicator;
        [SerializeField] private Transform _top;
        [SerializeField] private Transform _bottom;

        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Light _light;
        [SerializeField] private Material _offMaterial;
        [SerializeField] private Material _onMaterial;
        [SerializeField] private Color _offLightColor;
        [SerializeField] private Color _onLightColor;

        private MotionHandle _handle;

        public void Initialize(float slack)
        {
            _chargeIndicator.transform.position = Vector3.Lerp(_bottom.position, _top.position, _charge);
            _lowerIndicator.transform.position = Vector3.Lerp(_bottom.position, _top.position, _target - slack);
            _upperIndicator.transform.position = Vector3.Lerp(_bottom.position, _top.position, _target + slack);
        }

        public bool IsRegulated(float slack) => Mathf.Abs(_target - _charge) <= slack;

        public void Pump(float gain)
        {
            float currentCharge = _charge;

            _charge += gain;
            _charge = Mathf.Min(_charge, 1f);

            float newCharge = _charge;

            Vector3 start = Vector3.Lerp(_bottom.position, _top.position, currentCharge);
            Vector3 end = Vector3.Lerp(_bottom.position, _top.position, newCharge);
            _handle.TryCancel();
            _handle = LMotion.Create(start, end, 0.5f)
                .WithEase(Ease.OutQuad)
                .BindToPosition(_chargeIndicator.transform);
        }

        public void Drop(float dropAmount)
        {
            float currentCharge = _charge;

            _charge -= dropAmount;
            _charge = Mathf.Max(_charge, 0f);

            float newCharge = _charge;

            Vector3 start = Vector3.Lerp(_bottom.position, _top.position, currentCharge);
            Vector3 end = Vector3.Lerp(_bottom.position, _top.position, newCharge);
            _handle.TryCancel();
            _handle = LMotion.Create(start, end, 0.5f)
                .WithEase(Ease.Linear)
                .BindToPosition(_chargeIndicator.transform);
        }

        public void Set(bool on)
        {
            _renderer.material = on ? _onMaterial : _offMaterial;
            _light.enabled = on;
            _light.color = on ? _onLightColor : _offLightColor;
        }
    }
}