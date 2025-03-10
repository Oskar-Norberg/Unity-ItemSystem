using Project.ItemSystem;
using Project.PlayerCharacter.Item;
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

        // TODO: Add grab point to item
        public virtual void Equip(Transform equipper)
        {
            if (!equipper.TryGetComponent<PlayerItemHolder>(out var playerItemHolder))
            {
                Debug.LogWarning("Could not equip item, equipper does not have a PlayerItemHolder component");
                return;
            }

            playerItemHolder.Equip(this);
        }
    }
}