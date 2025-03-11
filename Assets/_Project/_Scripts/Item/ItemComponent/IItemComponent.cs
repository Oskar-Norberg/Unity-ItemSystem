using UnityEngine;

namespace Project.ItemSystem.Components
{
    // TODO: Consider making this a class to use RequireComponent.
    public interface IItemComponent
    {
        public void Use(Transform user);
        
        public Item Item { get; }
    }
}
