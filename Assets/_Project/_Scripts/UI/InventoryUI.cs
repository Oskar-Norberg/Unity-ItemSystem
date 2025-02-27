using System.Text;
using Project.PlayerCharacter;
using TMPro;
using UnityEngine;

namespace Project.InventorySystem.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        
        private Inventory _inventoryToDisplay;
        private bool _isInventoryOpen;
        
        private void OnEnable()
        {
            _isInventoryOpen = gameObject.activeInHierarchy;
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
            }
            else
            {
                
            }
        }

        // TODO: VERY TEMPORARY DEBUG DISPLAY
        private void SetText(Inventory inventory)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in inventory.GetItems())
            {
                sb.Append(item.itemName + "\n");
            }
            
            text.text = sb.ToString();
        }
    }
}