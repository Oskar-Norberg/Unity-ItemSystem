using System.Collections.Generic;
using Project.ItemSystem;
using UnityEngine;

namespace Project.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        private List<InventoryItem> _items = new();
        
        /**
         * <summary>Pick up / Add item</summary>
         * <param name="itemData">ItemData for item to pick up</param>
         * <returns>Returns whether item was sucessfully picked up</returns>
         */
        public bool AddItem(ItemData itemData)
        {
            InventoryItem inventoryItem;
            
            if (TryGetItem(itemData, out inventoryItem))
            {
                // TODO: Implement max stack size
                if (itemData is StackableItemData)
                    inventoryItem.amount++;
                else
                    _items.Add(inventoryItem);
            }
            else
            {
                inventoryItem = new InventoryItem(itemData, 1);
                _items.Add(inventoryItem);
            }
            
            // TODO: Implement if item was added successfully
            return true;
        }

        public List<InventoryItem> GetItems()
        {
            return _items;
        }

        private bool TryGetItem(ItemData itemData, out InventoryItem inventoryItem)
        {
            foreach (var item in _items)
            {
                if (item.itemData == itemData)
                {
                    inventoryItem = item;
                    return true;
                }
            }

            inventoryItem = null;
            return false;
        }
    }
    
    
}

