using Project.ItemSystem;
using Project.PlayerCharacter;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.InventorySystem.UI
{
    public class InventoryEntry : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private Image image;

        private InventorySlot _inventorySlot;
        private Sprite _defaultSprite;

        // Dragging and dropping
        private Transform _originalParent;
        private Vector3 _originalPosition;

        private void Awake()
        {
            _defaultSprite = image.sprite;
        }

        // TODO: make it so you cant drag and drop empty slots
        public void OnBeginDrag(PointerEventData eventData)
        {
            StartDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Drag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDrag();
        }

        public void OnDrop(PointerEventData eventData)
        {
            PlayerInventory.Instance.InventoryNavigationManager.OnFinishDrag(_inventorySlot);
        }

        public void SetInventorySlot(InventorySlot inventorySlot)
        {
            if (_inventorySlot != null)
                _inventorySlot.OnItemSet -= UpdateItem;

            _inventorySlot = inventorySlot;
            inventorySlot.OnItemSet += UpdateItem;
            UpdateItem(inventorySlot);
        }

        public void Equip()
        {
            _inventorySlot.Equip();
        }

        public void Drop()
        {
            _inventorySlot.Drop();
        }

        private void UpdateItem(InventorySlot inventorySlot)
        {
            ResetEntry();

            if (inventorySlot == null || inventorySlot.ItemData == null)
                return;

            if (inventorySlot.ItemData is StackableItemData)
                countText.text = "x" + inventorySlot.Amount;

            if (inventorySlot.ItemData.sprite)
                image.sprite = inventorySlot.ItemData.sprite;
        }

        private void ResetEntry()
        {
            countText.text = string.Empty;
            image.sprite = _defaultSprite;
        }

        private void StartDrag(PointerEventData eventData)
        {
            _originalParent = image.transform.parent;
            _originalPosition = image.transform.position;
            image.transform.SetParent(transform.root);
            image.raycastTarget = false;

            PlayerInventory.Instance.InventoryNavigationManager.OnBeginDrag(_inventorySlot);
        }

        private void Drag(PointerEventData eventData)
        {
            image.transform.position = eventData.position;
        }

        private void EndDrag()
        {
            image.transform.SetParent(_originalParent);
            image.transform.position = _originalPosition;
            image.raycastTarget = true;
        }
    }
}