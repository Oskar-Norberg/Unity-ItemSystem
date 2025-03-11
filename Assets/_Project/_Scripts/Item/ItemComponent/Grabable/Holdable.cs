using Project.InteractableSystem;
using UnityEngine;

namespace Project.ItemSystem.Components
{
    public class Holdable : ItemComponent
    {
        public void Hold(Transform holder)
        {
            if (!holder.TryGetComponent<ItemHolder>(out var playerItemHolder))
            {
                Debug.LogWarning("Could not equip item, equipper does not have a ItemHolder component");
                return;
            }
        
            playerItemHolder.Grab(this);
        }
    }
}