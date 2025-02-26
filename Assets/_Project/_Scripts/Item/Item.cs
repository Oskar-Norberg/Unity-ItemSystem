using Project.ItemSystem;
using UnityEngine;

namespace Project.InteractableSystem
{
    public class Item : Interactable
    {
        [SerializeField] private ItemData itemData;
        
        public override void Interact(Transform interactor)
        {
            print("Item interacted with: " + itemData.itemName);
            InteractionFinished();
        }
    }
}