using Project.InteractableSystem;
using UnityEngine;

namespace Project.ItemSystem
{
    public class Item : Interactable
    {
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
    }
}