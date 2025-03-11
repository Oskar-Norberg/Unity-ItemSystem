using UnityEngine;

namespace Project.InteractableSystem
{
    public class ConsumableItem : Item
    {
        public override void Use(Transform user)
        {
            // do something
            Debug.Log("Consumed Item");
            
            Destroy(gameObject);
        }
    }
}
