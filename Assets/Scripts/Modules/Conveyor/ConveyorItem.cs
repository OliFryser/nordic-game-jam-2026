using UnityEngine;

namespace Modules.Conveyor
{
    public class ConveyorItem : MonoBehaviour
    {

        private float ItemBottom => transform.position.y - transform.localScale.y / 2; 
        
        public ConveyorBelt CurrentBelt;
        public ConveyorItemType ItemType;

        public bool IsBeingGrabbed { get; set; }
        
        public bool IsTouchingBelt()
        {
            return Mathf.Approximately(ItemBottom, CurrentBelt.GetSurface());
        }
        

        public bool IsWithinXBounds(Vector2 xBounds)
            => transform.position.x + transform.localScale.x / 2 >= xBounds.x 
               && transform.position.x - transform.localScale.x / 2 <= xBounds.y;

        public bool MoveItemAlongBelt(float deltaTime, float beltSpeed)
        {
            Vector3 direction = CurrentBelt.GetDirection();
            transform.Translate(direction * (beltSpeed * deltaTime));
            return !IsWithinXBounds(CurrentBelt.GetXBoundary());
        }

        public void FallTowardsBelt(float deltaTime, float fallSpeed, bool skipSurfaceCheck = false)
        {
            transform.Translate(Vector3.down * (fallSpeed * deltaTime));
            if (skipSurfaceCheck)
                return;
            
            if (ItemBottom < CurrentBelt.GetSurface())
            {
                transform.position = new Vector3(transform.position.x, CurrentBelt.GetSurface() + transform.localScale.y / 2, transform.position.z);
            }
        }
    }
}