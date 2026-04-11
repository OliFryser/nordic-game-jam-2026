using UnityEngine;

public class LightBulb : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Material _on;
    [SerializeField] private Material _off;
    [SerializeField] private Material _playerOn;
    [SerializeField] private Light _light;
    [SerializeField] private Color _onColor;
    [SerializeField] private Color _playerOnColor;
    
    
    public void TurnOn(bool player = false)
    {
        if (_on == null)
        {
            Debug.LogError($"Null");
            return;
        }
        
        _renderer.material = player ? _playerOn : _on;
        _light.enabled = true;
        _light.color = player ? _playerOnColor : _onColor;
    }

    public void TurnOff()
    {
        _renderer.material = _off;
        _light.enabled = false;
    }
}