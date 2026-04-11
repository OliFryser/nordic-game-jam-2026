using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Modules.Conveyor
{
    public enum ConveyorDirection
    {
        Left,
        Right
    }
    
    public class ConveyorBelt : MonoBehaviour
    {
        [SerializeField] private float _width;
        [SerializeField] private ConveyorDirection _direction;
        private float WheelRadius => 0.18f;
        
        public Vector2 GetXBoundary()
        {
            float xScale = _width / 2;
            return new Vector2(transform.position.x - xScale - WheelRadius * 2, transform.position.x + xScale + WheelRadius * 2);
        }

        public float GetSurface()
            => transform.position.y + WheelRadius;

        public Vector3 GetDirection()
        {
            return _direction == ConveyorDirection.Left ? Vector3.left : Vector3.right;
        }
    }
}