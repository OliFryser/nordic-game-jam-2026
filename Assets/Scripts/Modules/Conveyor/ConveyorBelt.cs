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
        
        [SerializeField] ConveyorDirection _direction;
        
        public Vector2 GetXBoundary()
        {
            float xScale = transform.localScale.x / 2;
            return new Vector2(transform.position.x - xScale, transform.position.x + xScale);
        }

        public float GetSurface()
            => transform.position.y + transform.localScale.y / 2;

        public Vector3 GetDirection()
        {
            return _direction == ConveyorDirection.Left ? Vector3.left : Vector3.right;
        }
    }
}