using System.Collections.Generic;
using Project.PlayerCharacter;
using UnityEngine;
using UnityEngine.Events;

namespace Project.InventorySystem.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryEntryPrefab;
        
        private Inventory _inventoryToDisplay;
        private bool _isInventoryOpen;

        [SerializeField] private UnityEvent onInventoryStarted;
        [SerializeField] private UnityEvent onInventoryOpened;
        [SerializeField] private UnityEvent onInventoryClosed;

        private bool _isInitialized;
        private List<InventoryEntry> _inventoryEntries = new();

        #region Events Subscription
        private void Start()
        {
            onInventoryStarted?.Invoke();
        }
        
        private void OnEnable()
        {
            PlayerInventory.OnInventoryEvent += ToggleInventory;
        }

        private void OnDisable()
        {
            PlayerInventory.OnInventoryEvent -= ToggleInventory;
        }
        #endregion
        
        private void ToggleInventory(Inventory inventory)
        {
            _isInventoryOpen = !_isInventoryOpen;
            
            if (_isInventoryOpen)
                EnableInventory(inventory);
            else
                DisableInventory(inventory);
        }

        private void Initialize(Inventory inventory)
        {
            if (_isInitialized)
                return;

            _inventoryToDisplay = inventory;
            _isInitialized = true;

            foreach (var inventorySlot in inventory.GetInventorySlots())
            {
                GameObject inventoryEntry = Instantiate(inventoryEntryPrefab, transform);
                InventoryEntry inventoryEntryComponent = inventoryEntry.GetComponent<InventoryEntry>();

                _inventoryEntries.Add(inventoryEntryComponent);
                inventoryEntryComponent.SetItem(inventorySlot);
            }
        }

        private void EnableInventory(Inventory inventory)
        {
            _inventoryToDisplay = inventory;
            onInventoryOpened?.Invoke();
            
            Initialize(inventory);

            SetEntriesActive(true);
        }

        private void DisableInventory(Inventory inventory)
        {
            onInventoryClosed?.Invoke();

            SetEntriesActive(false);
        }

        private void SetEntriesActive(bool active)
        {
            foreach (var entry in _inventoryEntries)
            {
                entry.gameObject.SetActive(active);
            }
        }
    }
}