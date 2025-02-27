using Project.InventorySystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.PlayerCharacter
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInventory : Inventory
    {
        public delegate void OnInventoryEventHandler(Inventory inventory);
        public static event OnInventoryEventHandler OnInventoryEvent;
        
        private void OnInventory(InputValue submitValue)
        {
            if (!submitValue.isPressed)
                return;
            
            OnInventoryEvent?.Invoke(this);
        }
    }
}