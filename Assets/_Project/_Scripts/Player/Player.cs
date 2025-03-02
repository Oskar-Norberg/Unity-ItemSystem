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
        
        private PlayerMovement _playerMovement;
        private PlayerInteraction _playerInteraction;
    
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
            
            _playerMovement = GetComponent<PlayerMovement>();
            _playerInteraction = GetComponent<PlayerInteraction>();
        }
    }
}