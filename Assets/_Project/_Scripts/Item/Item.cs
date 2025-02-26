using UnityEngine;

namespace Project.Item
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