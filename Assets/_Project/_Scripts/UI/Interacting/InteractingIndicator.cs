using Project.InteractableSystem;
using Project.PlayerCharacter;
using UnityEngine;
using UnityEngine.UI;

namespace Project.InventorySystem.UI
{
    public class InteractingIndicator : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        private Sprite _defaultSprite;

        private void Awake()
        {
            _defaultSprite = image.sprite;
        }

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

            var sprite = interactable is Item item ? item.ItemData.sprite : _defaultSprite;
            image.sprite = sprite;
        }
    }
}