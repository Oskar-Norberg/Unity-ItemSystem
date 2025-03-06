using Project.InventorySystem;
using Project.InventorySystem.UI;
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

        public InventoryNavigationManager InventoryNavigationManager { get; private set; }

        public delegate void OnInventoryEventHandler(Inventory inventory);
        public event OnInventoryEventHandler OnInventoryEvent;

        private new void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            base.Awake();
            
            InventoryNavigationManager = new InventoryNavigationManager(this);
        }

        private void OnInventory(InputValue submitValue)
        {
            if (!submitValue.isPressed)
                return;

            OnInventoryEvent?.Invoke(this);
        }

        [RuntimeInitializeOnLoadMethod]
        private void InitializeOnLoad()
        {
            _instance = null;
        }
    }
}