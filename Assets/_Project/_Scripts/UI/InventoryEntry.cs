using Project.ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.InventorySystem.UI
{
    public class InventoryEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private Image image;
        
        private InventorySlot _inventorySlot;
        
        // TODO: Rename this to SetInventorySlot, doesnt set an item. It sets the slot to represent.
        public void SetInventorySlot(InventorySlot inventorySlot)
        {
            inventorySlot.OnItemSet += UpdateItem;
            UpdateItem(inventorySlot);
        }

        private void UpdateItem(InventorySlot inventorySlot)
        {
            if (inventorySlot == null || inventorySlot.ItemData == null)
                return;
            
            if (inventorySlot.ItemData is StackableItemData)
                countText.text = "x" + inventorySlot.Amount;
            
            image.sprite = inventorySlot.ItemData.sprite;
        }
    }
}