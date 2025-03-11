using UnityEngine;

namespace Project.ItemSystem.Components
{
    [RequireComponent(typeof(Item))]
    public abstract class ItemComponent : MonoBehaviour
    {
        public Item Item { get; private set; }

        private void Awake()
        {
            Item = GetComponent<Item>();
        }

        private void OnEnable()
        {
            Item.OnItemUsed += Use;
        }

        private void OnDisable()
        {
            Item.OnItemUsed -= Use;
        }
        
        protected virtual void Use(Transform user){}
    }
}
