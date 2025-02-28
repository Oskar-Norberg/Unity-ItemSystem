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
        private Sprite _defaultSprite;

        private void Awake()
        {
            _defaultSprite = image.sprite;
        }

        // TODO: Rename this to SetInventorySlot, doesnt set an item. It sets the slot to represent.
        public void SetInventorySlot(InventorySlot inventorySlot)
        {
            if (_inventorySlot != null)
                _inventorySlot.OnItemSet -= UpdateItem;
            
            _inventorySlot = inventorySlot;
            inventorySlot.OnItemSet += UpdateItem;
            UpdateItem(inventorySlot);
        }

        public void Drop()
        {
            _inventorySlot.Drop();
        }

        private void UpdateItem(InventorySlot inventorySlot)
        {
            if (inventorySlot == null)
                return;

            if (inventorySlot.ItemData == null)
            {
                countText.text = string.Empty;
                image.sprite = _defaultSprite;
            }
            else
            {
                if (inventorySlot.ItemData is StackableItemData)
                    countText.text = "x" + inventorySlot.Amount;
            
                if (inventorySlot.ItemData.sprite)
                    image.sprite = inventorySlot.ItemData.sprite;
            }
        }
    }
}