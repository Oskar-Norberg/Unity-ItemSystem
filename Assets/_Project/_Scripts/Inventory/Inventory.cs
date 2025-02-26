using System.Collections.Generic;
using Project.ItemSystem;
using UnityEngine;

namespace Project.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        private List<ItemData> _items = new();
        
        /**
         * <summary>Pick up / Add item</summary>
         * <param name="itemData">ItemData for item to pick up</param>
         * <returns>Returns whether item was sucessfully picked up</returns>
         */
        public bool AddItem(ItemData itemData)
        {
            _items.Add(itemData);
            // TODO: Implement if item was added successfully
            return true;
        }
    }
}

