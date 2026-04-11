using UnityEngine;

public class LightBulb : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Material _on;
    [SerializeField] private Material _off;
    [SerializeField] private Light _light;
    
    public void TurnOn()
    {
        _renderer.material = _on;
        _light.enabled = true;
    }

    public void TurnOff()
    {
        _renderer.material = _off;
        _light.enabled = false;
    }
}