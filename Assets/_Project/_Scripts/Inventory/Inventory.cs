using System.Collections.Generic;
using Project.ItemSystem;
using UnityEngine;

namespace Project.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public int MaxInventorySize => maxInventorySize;
        
        [SerializeField] private int maxInventorySize = 25;
        private List<InventoryItem> _items = new();

        private void Awake()
        {
            for (int i = 0; i < maxInventorySize; i++)
            {
                _items.Add(new InventoryItem(null, 0));
            }
        }
        
        /**
         * <summary>Pick up / Add item</summary>
         * <param name="itemData">ItemData for item to pick up</param>
         * <returns>Returns whether item was sucessfully picked up</returns>
         */
        public bool AddItem(ItemData itemData)
        {
            if (itemData is StackableItemData)
                return AddStackableItem(itemData as StackableItemData);
            else
                return AddNonStackableItem(itemData);
        }

        private bool AddStackableItem(StackableItemData itemData)
        {
            // Find if an existing, non-full stack exists
            foreach (var item in _items)
            {
                if (item.ItemData == itemData)
                {
                    if (item.Amount < itemData.maxStack)
                    {
                        item.IncreaseStackSize(1);
                        return true;
                    }
                }
            }
            
            // If no existing stack exists, try to add a new stack
            return TryAddItem(itemData);
        }
        
        private bool AddNonStackableItem(ItemData itemData)
        {
            return TryAddItem(itemData);
        }

        public InventoryItem GetItem(int index)
        {
            return _items[index];
        }

        public List<InventoryItem> GetItems()
        {
            return _items;
        }

        private bool TryAddItem(ItemData itemdata)
        {
            foreach (var item in _items)
            {
                if (item.ItemData == null)
                {
                    item.SetItem(itemdata, 1);
                    return true;
                }
            }

            return false;
        }

        private bool TryGetItem(ItemData itemData, out InventoryItem inventoryItem)
        {
            foreach (var item in _items)
            {
                if (item.ItemData == itemData)
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
