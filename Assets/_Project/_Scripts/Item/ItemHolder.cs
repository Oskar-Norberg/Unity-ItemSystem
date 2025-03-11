using Project.InventorySystem;
using UnityEngine;

namespace Project.InteractableSystem
{
    public abstract class ItemHolder : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private Transform itemHolder;
        
        protected Item CurrentItem;
        
        public void Equip(Item item)
        {
            Dequip();
            
            item.transform.SetParent(itemHolder);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            
            CurrentItem = item;
        }

        protected void Dequip()
        {
            if (!CurrentItem)
                return;
            
            inventory.AddItem(CurrentItem.ItemData);

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