using UnityEngine;

namespace Project.PlayerCharacter.Item
{
    public class PlayerItemHolder : MonoBehaviour
    {
        [SerializeField] private Transform itemHolder;
        
        private InteractableSystem.Item _currentItem;
        
        public void Equip(InteractableSystem.Item item)
        {
            if (_currentItem)
                Destroy(_currentItem.gameObject);
            
            item.transform.SetParent(itemHolder);
            item.transform.localPosition = Vector3.zero;
            
            _currentItem = item;
        }
    }
}