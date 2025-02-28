using System.Collections.Generic;
using Project.ItemSystem;
using UnityEngine;

namespace Project.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public int MaxInventorySize => maxInventorySize;
        
        [SerializeField] private int maxInventorySize = 25;
        private List<InventorySlot> _inventorySlots = new();

        private void Awake()
        {
            for (int i = 0; i < maxInventorySize; i++)
            {
                _inventorySlots.Add(new InventorySlot(null, 0));
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
            foreach (var item in _inventorySlots)
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

        public InventorySlot GetInventorySlot(int index)
        {
            return _inventorySlots[index];
        }

        public List<InventorySlot> GetInventorySlots()
        {
            return _inventorySlots;
        }

        private bool TryAddItem(ItemData itemdata)
        {
            foreach (var item in _inventorySlots)
            {
                if (item.ItemData == null)
                {
                    item.SetItem(itemdata, 1);
                    return true;
                }
            }

            return false;
        }

        private bool TryGetItem(ItemData itemData, out InventorySlot inventorySlot)
        {
            foreach (var item in _inventorySlots)
            {
                if (item.ItemData == itemData)
                {
                    inventorySlot = item;
                    return true;
                }
            }

            inventorySlot = null;
            return false;
        }
    }
}
