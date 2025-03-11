using Project.InteractableSystem;
using UnityEngine;

namespace Project.ItemSystem.Components
{
    public class Grabable : ItemComponent
    {
        public void Hold(Transform equipper)
        {
            if (!equipper.TryGetComponent<ItemHolder>(out var playerItemHolder))
            {
                Debug.LogWarning("Could not equip item, equipper does not have a ItemHolder component");
                return;
            }
        
            playerItemHolder.Grab(this);
        }
    }
}