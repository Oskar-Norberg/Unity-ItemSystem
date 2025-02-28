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

        private List<InventoryEntry> _inventoryEntries = new();

        #region Events Subscription
        private void Start()
        {
            Initialize(PlayerInventory.Instance);

            SetEntriesActive(false);
            onInventoryStarted?.Invoke();
        }
        
        private void OnEnable()
        {
            PlayerInventory.Instance.OnInventoryEvent += ToggleInventory;
        }

        private void OnDisable()
        {
            if (PlayerInventory.Instance != null)
                PlayerInventory.Instance.OnInventoryEvent -= ToggleInventory;
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
            if (inventory.InventorySize == _inventoryEntries.Count)
                return;

            // Create more entries if inventory size is greater than the current amount of entries
            if (inventory.InventorySize >= _inventoryEntries.Count)
                CreateNewEntries(inventory);
            
            // Hide entries that are not needed
            if (inventory.InventorySize < _inventoryEntries.Count)
                HideExtraEntries(inventory);

            _inventoryToDisplay = inventory;

            LinkInventoryEntries(inventory);
        }

        private void CreateNewEntries(Inventory inventory)
        {
            for (int i = _inventoryEntries.Count; i < inventory.InventorySize; i++)
            {
                _inventoryEntries.Add(CreateNewEntry());
            }
        }

        private InventoryEntry CreateNewEntry()
        {
            GameObject inventoryEntry = Instantiate(inventoryEntryPrefab, transform);
            InventoryEntry inventoryEntryComponent = inventoryEntry.GetComponent<InventoryEntry>();

            return inventoryEntryComponent;
        }

        private void HideExtraEntries(Inventory inventory)
        {
            for (int i = inventory.InventorySize; i < _inventoryEntries.Count; i++)
            {
                SetEntryActive(_inventoryEntries[i], false);
            }
        }

        private void SetEntryActive(InventoryEntry entry, bool active)
        {
            entry.gameObject.SetActive(active);
        }

        private void LinkInventoryEntries(Inventory inventory)
        {
            InventorySlot[] inventorySlots = inventory.GetInventorySlots();

            for (int i = 0; i < inventorySlots.Length; i++)
            {
                _inventoryEntries[i].SetItem(inventorySlots[i]);
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
            for (int i = 0; i < _inventoryToDisplay.InventorySize; i++)
            {
                if (_inventoryEntries[i] == null)
                    continue;
                
                SetEntryActive(_inventoryEntries[i], active);
            }
        }
    }
}