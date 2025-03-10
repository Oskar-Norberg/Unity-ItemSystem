using UnityEngine;

namespace Project.InteractableSystem
{
    public abstract class ItemHolder : MonoBehaviour
    {
        [SerializeField] private Transform itemHolder;
        
        protected Item CurrentItem;
        
        public void Equip(Item item)
        {
            DestroyItem();
            
            item.transform.SetParent(itemHolder);
            item.transform.localPosition = Vector3.zero;
            
            CurrentItem = item;
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