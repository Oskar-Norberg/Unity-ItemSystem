using UnityEngine;

namespace Project.PlayerCharacter.Item
{
    public class PlayerItemHolder : MonoBehaviour
    {
        [SerializeField] private Transform itemHolder;
        
        private InteractableSystem.Item _currentItem;
        
        public void Equip(InteractableSystem.Item item)
        {
            item.transform.SetParent(transform);
            
        }
    }
}