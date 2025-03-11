using UnityEngine;

namespace Project.InteractableSystem
{
    public class WeaponItem : Item
    {
        public override void Use(Transform user)
        {
            Debug.Log("Fire " + ItemData.name);
        }
    }
}