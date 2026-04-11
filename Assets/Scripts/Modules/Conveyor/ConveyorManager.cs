using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Input;
using UnityEngine;
using Random = System.Random;

namespace Modules.Conveyor
{
    public class ConveyorManager : MonoBehaviour
    {
        [SerializeField] private EngineButtonPressEmitter _engineButtonPressEmitter;
        [SerializeField] private Transform _itemSpawnerTransform;
        [SerializeField] private Transform _itemDestroyerTransform;
        [SerializeField] private ConveyorBelt _lowerBelt;
        [SerializeField] private ConveyorBelt _upperBelt;
        [SerializeField] private List<ConveyorGrabber> _grabbers;

        [Header("Conveyor Speeds")] 
        [SerializeField]
        private float _fallSpeed = 8f;
        [SerializeField]
        private float _beltSpeed = 5f;

        [SerializeField] private float _spawningDelay = 1f;
        
        [SerializeField]
        private List<ConveyorItem> _items;

        [SerializeField] private List<GameObject> _itemModels;

        private void Start()
        {
            AssignTypesToItemsAndGrabbers();
            _items.Shuffle(new Random());
            SpawnItemsWithDelay();
        }

        private void AssignTypesToItemsAndGrabbers()
        {
            for (int i = 0; i < _grabbers.Count; i++)
            {
                ConveyorGrabber grabber = _grabbers[i];
                ConveyorItem item = _items[i];
                int typeIndex = i / Enum.GetValues(typeof(ConveyorItemType)).Length;
                ConveyorItemType type = (ConveyorItemType)typeIndex;
                grabber.CompatibleType = type;
                GameObject prefab = _itemModels[typeIndex];
                item.ItemType = type;
                Instantiate(prefab, item.transform);
            }
        }

        private async Awaitable SpawnItemsWithDelay()
        {
            List<ConveyorItem> conveyorItems = _items.ToList();
            foreach (var conveyorItem in conveyorItems)
            {
                if (!conveyorItem)
                    return;
                
                conveyorItem.transform.position = _itemSpawnerTransform.position;
                conveyorItem.gameObject.SetActive(true);
                await Awaitable.WaitForSecondsAsync(_spawningDelay);
            }
        }
        
        private void OnEnable()
        {
            _engineButtonPressEmitter.OnPress += OnEngineButtonPress;
        }

        private void OnDisable()
        {
            _engineButtonPressEmitter.OnPress -= OnEngineButtonPress;
        }

        private void Update()
        {
            if (_grabbers.TrueForAll(g => g.HasCollectedItemType))
            {
                return;
            }
            MoveConveyorItems();
        }

        private void MoveConveyorItems()
        {
            foreach (var conveyorItem in _items)
            {
                if (!conveyorItem.gameObject.activeSelf || conveyorItem.IsBeingGrabbed)
                    continue;
                if (conveyorItem.CurrentBelt == null)
                {
                    conveyorItem.FallTowardsBelt(Time.deltaTime, _beltSpeed, skipSurfaceCheck: true);
                    if (conveyorItem.transform.position.y < _itemDestroyerTransform.position.y)
                    {
                        RespawnItem(conveyorItem);
                    }
                }
                else if (conveyorItem.IsTouchingBelt())
                {
                    var fellOff = conveyorItem.MoveItemAlongBelt(Time.deltaTime, _beltSpeed);
                    if (!fellOff)
                        continue;
                    conveyorItem.CurrentBelt = conveyorItem.CurrentBelt == _upperBelt ? _lowerBelt : null;
                }
                else
                {
                    conveyorItem.FallTowardsBelt(Time.deltaTime, _fallSpeed);
                }
            }
        }

        private void RespawnItem(ConveyorItem conveyorItem)
        {
            conveyorItem.CurrentBelt = _upperBelt;
            conveyorItem.transform.localPosition = _itemSpawnerTransform.position;
        }

        private async void OnEngineButtonPress(EngineButton button)
        {
            if (button.Section != DashboardSection.ConveyorBelt)
                return;
            
            ConveyorGrabber activatedGrabber = _grabbers[button.InSectionIndex];
            
            if (activatedGrabber.HasCollectedItemType)
                return;
            
            ConveyorItem item = 
                activatedGrabber.TryGetValidItemUnderneath(
                    _items.Where(i => i.ItemType == activatedGrabber.CompatibleType));

            if (item != null)
            {
                await activatedGrabber.StartGrab(item);
                _items.Remove(item);
                Destroy(item.gameObject);
            }
        }
    }
}