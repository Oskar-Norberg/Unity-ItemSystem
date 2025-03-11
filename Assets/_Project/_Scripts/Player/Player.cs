using Project.PlayerCharacter.Item;
using UnityEngine;

namespace Project.PlayerCharacter
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerInteractor))]
    [RequireComponent(typeof(PlayerItemHolder))]
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
        public PlayerInteractor PlayerInteraction { get; private set; }
        public PlayerItemHolder PlayerItemHolder { get; private set; }
    
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
            PlayerInteraction = GetComponent<PlayerInteractor>();
            PlayerItemHolder = GetComponent<PlayerItemHolder>();
        }
        
        [RuntimeInitializeOnLoadMethod]
        private void InitializeOnLoad()
        {
            _instance = null;
        }
    }
}