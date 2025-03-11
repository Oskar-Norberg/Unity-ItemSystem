using UnityEngine;

namespace Project.ItemSystem.Components
{
    public class Consumable : MonoBehaviour, IItemComponent
    {
        public Item Item { get; private set; }

        // TODO: This code is getting repeated a lot, consider moving it to a base class
        private void Start()
        {
            Item = GetComponent<Item>();
        }

        public void Use(Transform user)
        {
            Consume(user);
        }

        private void Consume(Transform consumer)
        {
            Debug.Log("Consumed item " + Item.name);
        }
    }
}