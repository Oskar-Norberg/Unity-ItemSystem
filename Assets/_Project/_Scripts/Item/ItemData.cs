using UnityEngine;

namespace Project.ItemSystem
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/Items/Item Data")]
    public class ItemData : ScriptableObject
    {
        public new string name;
        public Sprite sprite;
    
        public GameObject prefab;
    }
}