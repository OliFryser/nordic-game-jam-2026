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

        private GrabberArm GrabberArm { get; set; }
        
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

        public async Awaitable StartGrab(ConveyorItem item)
        {
            item.IsBeingGrabbed = true;
            GrabberArm.ExtendArm();
            await Awaitable.WaitForSecondsAsync(0.2f);
            GrabberArm.RetractArm(item.transform);
            await Awaitable.WaitForSecondsAsync(0.2f);
        }

        private void OnCollectedItem()
        {
            HasCollectedItemType = true;
            GetComponent<Renderer>().material.color = Color.green;
        }
        
        private bool IsUnder(ConveyorItem item)
        {
            Vector3 distanceVector = item.transform.position - transform.position;
            return distanceVector.y < 0;
        }
    }
}