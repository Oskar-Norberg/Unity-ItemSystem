using UnityEngine;

namespace Project.ItemSystem
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/Items/Item Data")]
    public class ItemData : ScriptableObject
    {
        // TODO: Remove item prefix, it's redundant
        public string itemName;
        public Sprite itemSprite;
    
        public GameObject itemPrefab;
    }
}