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

        private void Update()
        {
            var interactable = Player.Instance.PlayerInteraction.CurrentlyClosestInteractable;
            image.enabled = interactable != null;
            image.sprite = interactable is Item item ? item.ItemData.sprite : _defaultSprite;
        }
    }
}