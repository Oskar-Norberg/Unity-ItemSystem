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
            
            image.sprite = inventorySlot.ItemData.sprite;
        }
    }
}