using Project.ItemSystem;
using UnityEngine;

namespace Project.InteractableSystem
{
    public class Item : Interactable
    {
        [SerializeField] private ItemData itemData;
        
        public override void Interact(Transform interactor)
        {
            InteractionFinished();

            if (!interactor.TryGetComponent(out InventorySystem.Inventory inventory))
                return;
            
            inventory.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}