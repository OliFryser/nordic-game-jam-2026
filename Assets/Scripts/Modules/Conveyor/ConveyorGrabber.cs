using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Modules.Conveyor
{
    public class ConveyorGrabber : MonoBehaviour
    {
        public ConveyorItemType CompatibleType;
        private Vector2 _xBounds { get; set; }
        
        [SerializeField]
        private GameObject _grabberModel;

        [SerializeField] private ConveyorBelt _belt;
        
        private GrabberArm GrabberArm { get; set; }
        
        private bool IsExtending { get; set; }
        
        public bool HasCollectedItemType { get; private set; }
        
        private void Awake()
        {
            float xScale = transform.localScale.x / 2;
            _xBounds = new Vector2(transform.position.x - xScale, transform.position.x + xScale);
        }

        private void Start()
        {
            GrabberArm = Instantiate(_grabberModel, transform).GetComponent<GrabberArm>();
        }
        
        [CanBeNull]
        public ConveyorItem TryGetValidItemUnderneath(IEnumerable<ConveyorItem> compatibleItems)
        {
            foreach (var compatibleItem in compatibleItems)
            {
                if (IsUnder(compatibleItem) && compatibleItem.IsWithinXBounds(_xBounds))
                {
                    OnCollectedItem();
                    return compatibleItem;
                }
            }
            return null;
        }

        public async Awaitable StartGrab(ConveyorItem item, float grabberSpeed)
        {
            if (IsExtending)
                return;
            IsExtending = true;
            item.transform.position = new Vector3(transform.position.x, item.transform.position.y, item.transform.position.z);
            item.IsBeingGrabbed = true;
            GrabberArm.ExtendArm(grabberSpeed);
            await Awaitable.WaitForSecondsAsync(grabberSpeed);
            GrabberArm.RetractArm(item.transform, grabberSpeed);
            await Awaitable.WaitForSecondsAsync(grabberSpeed);
            IsExtending = false;
        }

        private void OnCollectedItem()
        {
            HasCollectedItemType = true;
        }
        
        private bool IsUnder(ConveyorItem item)
        {
            float distanceToBeltSurface = transform.position.y - _belt.GetSurface();
            float distanceToItem = transform.position.y - item.transform.position.y;
            return distanceToItem > 0 && distanceToItem <= distanceToBeltSurface;
        }

        public async Awaitable StartFailGrab(float grabberSpeed)
        {
            if (IsExtending)
                return;
            IsExtending = true;
            GrabberArm.ExtendFailArm(grabberSpeed);
            await Awaitable.WaitForSecondsAsync(grabberSpeed);
            GrabberArm.RetractFailArm(grabberSpeed);
            await Awaitable.WaitForSecondsAsync(grabberSpeed);
            IsExtending = false;
        }
    }
}