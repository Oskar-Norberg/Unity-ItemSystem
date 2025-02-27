using System.Text;
using Project.PlayerCharacter;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Project.InventorySystem.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        
        private Inventory _inventoryToDisplay;
        private bool _isInventoryOpen;

        [SerializeField] private UnityEvent onInventoryStarted;
        [SerializeField] private UnityEvent onInventoryOpened;
        [SerializeField] private UnityEvent onInventoryClosed;

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

        public void ToggleInventory(Inventory inventory)
        {
            _isInventoryOpen = !_isInventoryOpen;
            
            if (_isInventoryOpen)
            {
                _inventoryToDisplay = inventory;
                SetText(inventory);
                onInventoryOpened?.Invoke();
            }
            else
            {
                onInventoryClosed?.Invoke();
            }
        }

        // TODO: VERY TEMPORARY DEBUG DISPLAY
        private void SetText(Inventory inventory)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in inventory.GetItems())
            {
                sb.AppendFormat("{0} x{1}\n", item.itemData.name, item.amount);
            }
            
            text.text = sb.ToString();
        }
    }
}