using Project.InventorySystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.PlayerCharacter
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInventory : Inventory
    {
        private void OnInventory(InputValue submitValue)
        {
            if (!submitValue.isPressed)
                return;
        }
    }
}