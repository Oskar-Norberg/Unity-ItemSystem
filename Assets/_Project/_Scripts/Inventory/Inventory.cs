using Project.ItemSystem;
using UnityEngine;

namespace Project.InventorySystem
{
    //TODO: Please sort the methods in this class in the order of their access modifiers
    public class Inventory : MonoBehaviour
    {
        public int InventorySize => _inventorySlots.Length;
        
        [SerializeField] private int startingInventorySize = 25;
        private InventorySlot[] _inventorySlots;

        protected void Awake()
        {
            _inventorySlots = new InventorySlot[startingInventorySize];
            
            for (int i = 0; i < startingInventorySize; i++)
            {
                _inventorySlots[i] = new InventorySlot(this, null, 0);
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
                    if (item.Amount < itemData.maxStackSize)
                    {
                        item.IncrementStackSize(1);
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

        public InventorySlot[] GetInventorySlots()
        {
            return _inventorySlots;
        }
        
        public void DropItem(InventorySlot inventorySlot)
        {
            Instantiate(inventorySlot.ItemData.prefab, transform.position, Quaternion.identity);
            
            inventorySlot.IncrementStackSize(-1);
            
            if (inventorySlot.Amount == 0)
                inventorySlot.SetItem(null, 0);
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

        public bool DragItem(InventorySlot from, InventorySlot to)
        {
            if (from.ItemData is StackableItemData && to.ItemData is StackableItemData)
                return DragStackableItem(from, to);
            else
                return DragNonStackableItem(from, to);
        }
        
        private bool DragStackableItem(InventorySlot from, InventorySlot to)
        {
            StackableItemData fromData = from.ItemData as StackableItemData;
            StackableItemData toData = to.ItemData as StackableItemData;
            
            if (fromData == null || toData == null)
                return false;
            
            int amountToMove = Mathf.Min(from.Amount, toData.maxStackSize - to.Amount);
            
            from.IncrementStackSize(-amountToMove);
            to.IncrementStackSize(amountToMove);
            
            if (from.Amount == 0)
                from.SetItem(null, 0);

            return true;
        }
        
        private bool DragNonStackableItem(InventorySlot from, InventorySlot to)
        {
            var tempToData = to.ItemData;
            var tempToAmount = to.Amount;
            
            to.SetItem(from.ItemData, from.Amount);
            
            from.SetItem(tempToData, tempToAmount);

            return true;
        }
    }
}
