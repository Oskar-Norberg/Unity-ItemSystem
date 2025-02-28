using Project.InventorySystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.PlayerCharacter
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInventory : Inventory
    {
        public static PlayerInventory Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogWarning("Instance of " + nameof(PlayerInventory) + " is null");
                    return null;
                }

                return _instance;
            }
        }
        
        private static PlayerInventory _instance;
        
        public delegate void OnInventoryEventHandler(Inventory inventory);
        public event OnInventoryEventHandler OnInventoryEvent;

        private new void Awake()
        {
            base.Awake();
            
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
        }
        
        private void OnInventory(InputValue submitValue)
        {
            if (!submitValue.isPressed)
                return;

            OnInventoryEvent?.Invoke(this);
        }
    }
}