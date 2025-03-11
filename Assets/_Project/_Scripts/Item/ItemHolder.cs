using Project.InventorySystem;
using Project.ItemSystem.Components;
using UnityEngine;

namespace Project.InteractableSystem
{
    public abstract class ItemHolder : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private Transform itemHolder;
        
        protected Grabable CurrentItem;
        
        public void Grab(Grabable item)
        {
            Drop();
            
            item.transform.SetParent(itemHolder);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            
            CurrentItem = item;
        }

        protected void Drop()
        {
            if (!CurrentItem)
                return;
            
            inventory.AddItem(CurrentItem.Item.ItemData);

            DestroyItem();
        }

        private void OnSubmit()
        {
            if (CurrentItem)
                CurrentItem.Use(transform);
        }

        protected void DestroyItem()
        {
            if (CurrentItem)
                Destroy(CurrentItem.gameObject);

            CurrentItem = null;
        }
    }
}