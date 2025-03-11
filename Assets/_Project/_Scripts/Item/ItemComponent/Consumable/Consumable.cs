using UnityEngine;

namespace Project.ItemSystem.Components
{
    public class Consumable : ItemComponent
    { 
        protected override void Use(Transform user)
        {
            Consume(user);
        }

        private void Consume(Transform consumer)
        {
            Debug.Log("Consumed item " + Item.name);
        }
    }
}