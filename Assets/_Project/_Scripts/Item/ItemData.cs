using UnityEngine;

namespace Project.Item
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/Item")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public Sprite itemSprite;
    
        public GameObject itemPrefab;
    }
}