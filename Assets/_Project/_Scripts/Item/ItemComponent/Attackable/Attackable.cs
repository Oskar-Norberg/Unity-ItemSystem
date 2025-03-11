using UnityEngine;

namespace Project.ItemSystem.Components
{
    public class Attackable : ItemComponent
    {
        protected override void Use(Transform user)
        {
            Attack(user);
        }

        private void Attack(Transform user)
        {
            // TODO: Implement
            Debug.Log("Attacked with item " + Item.name);
        }
    }
}