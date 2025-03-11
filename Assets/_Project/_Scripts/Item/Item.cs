using Project.InteractableSystem;
using UnityEngine;

namespace Project.ItemSystem
{
    public class Item : Interactable
    {
        public delegate void OnItemUsedEventHandler(Transform user);
        public event OnItemUsedEventHandler OnItemUsed;
        
        public ItemData ItemData => itemData;
        [SerializeField] private ItemData itemData;
        
        public override void Interact(Transform interactor)
        {
            InteractionFinished();

            if (!interactor.TryGetComponent(out InventorySystem.Inventory inventory))
                return;
            
            if (!inventory.AddItem(itemData))
                return;
            
            Destroy(gameObject);
        }

        public void Use(Transform user)
        {
            OnItemUsed?.Invoke(user);
        }
    }
}