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
        
        private InventoryItem _inventoryItem;
        
        public void SetItem(InventoryItem inventoryItem)
        {
            inventoryItem.OnItemSet += UpdateItem;
            UpdateItem(inventoryItem);
        }

        private void UpdateItem(InventoryItem inventoryItem)
        {
            if (inventoryItem == null || inventoryItem.ItemData == null)
                return;
            
            countText.text = inventoryItem.Amount.ToString();
            image.sprite = inventoryItem.ItemData.itemSprite;
        }
    }
}