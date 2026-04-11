using UnityEngine;

public class LightbulbMaterialController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Light _light;
    [SerializeField] private Material _offMaterial;
    [SerializeField] private Material _onMaterial;
    [SerializeField] private Color _offLightColor;
    [SerializeField] private Color _onLightColor;

    public void Set(bool on)
    {
        _renderer.material = on ? _onMaterial : _offMaterial;

        if (_light == null) return;
        
        _light.enabled = on;
        _light.color = on ? _onLightColor : _offLightColor;
    }
}