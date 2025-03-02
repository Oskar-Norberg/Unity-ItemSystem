using Project.InteractableSystem;
using Project.PlayerCharacter;
using UnityEngine;
using UnityEngine.UI;

namespace Project.InventorySystem.UI
{
    public class InteractingIndicator : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        private void OnEnable()
        {
            Player.Instance.PlayerInteraction.OnInteractableHover += OnInteractableHover;
        }
    
        private void OnDisable()
        {
            Player.Instance.PlayerInteraction.OnInteractableHover -= OnInteractableHover;
        }

        private void OnInteractableHover(Interactable interactable)
        {
            image.enabled = interactable != null;
        }
    }
}