using UnityEngine;

namespace Project.ItemSystem
{
    [CreateAssetMenu(fileName = "StackableItemData", menuName = "Scriptable Objects/Items/Stackable Item Data")]
    public class StackableItemData : ItemData
    {
        // TODO: Rename to maxStackSize
        public int maxStack;
    }
}