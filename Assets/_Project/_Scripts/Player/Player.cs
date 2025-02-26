using UnityEngine;

namespace Project.PlayerCharacter
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerInteraction))]
    public class Player : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private PlayerInteraction _playerInteraction;
    
        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerInteraction = GetComponent<PlayerInteraction>();
        }
    }
}