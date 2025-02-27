using UnityEngine;

namespace Project.ItemSystem
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/Item")]
    public class ItemData : ScriptableObject
    {
        // TODO: Remove item prefix, it's redundant
        public string itemName;
        public Sprite itemSprite;
    
        public GameObject itemPrefab;
        
        public bool isStackable;
        public int maxStack;
    }
}