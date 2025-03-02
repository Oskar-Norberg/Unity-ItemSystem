using Project.ItemSystem;
using UnityEngine;

namespace Project.InteractableSystem
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