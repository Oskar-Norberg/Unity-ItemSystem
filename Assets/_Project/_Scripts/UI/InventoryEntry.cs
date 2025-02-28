using Project.ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.InventorySystem.UI
{
    // TODO: These should be linked to inventory slots rather than the items themselves. Look into creating an observable list for the inventory.
    public class InventoryEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private Image image;
        
        private InventorySlot _inventorySlot;
        
        public void SetItem(InventorySlot inventorySlot)
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
            
            image.sprite = inventorySlot.ItemData.itemSprite;
        }
    }
}