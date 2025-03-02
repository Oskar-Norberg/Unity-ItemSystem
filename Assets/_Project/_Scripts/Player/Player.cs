using UnityEngine;

namespace Project.PlayerCharacter
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerInteraction))]
    public class Player : MonoBehaviour
    {
        public static Player Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogWarning("Instance of " + nameof(Player) + " is null");
                    return null;
                }

                return _instance;
            }
        }

        private static Player _instance;
        
        public PlayerMovement PlayerMovement { get; private set; }
        public PlayerInteraction PlayerInteraction { get; private set; }
    
        private void Awake()
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
            
            PlayerMovement = GetComponent<PlayerMovement>();
            PlayerInteraction = GetComponent<PlayerInteraction>();
        }
    }
}