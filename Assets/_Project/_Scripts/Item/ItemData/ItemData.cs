using UnityEngine;

namespace Project.ItemSystem
{
    public abstract class ItemData : ScriptableObject
    {
        public new string name;
        public Sprite sprite;
    
        public GameObject prefab;
    }
}