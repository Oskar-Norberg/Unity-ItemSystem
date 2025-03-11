using Project.ItemSystem;
using Project.PlayerCharacter.Item;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.InteractableSystem
{
    public abstract class Item : Interactable
    {
        public ItemData ItemData => itemData;
        [SerializeField] protected ItemData itemData;

        [SerializeField] protected Transform pickUpPoint;
        
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
            if (!equipper.TryGetComponent<ItemHolder>(out var playerItemHolder))
            {
                Debug.LogWarning("Could not equip item, equipper does not have a ItemHolder component");
                return;
            }

            playerItemHolder.Equip(this);
        }

        public abstract void Use(Transform user);
    }
}