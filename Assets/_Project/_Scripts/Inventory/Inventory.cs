using Project.InteractableSystem;
using Project.ItemSystem;
using UnityEngine;

namespace Project.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public int InventorySize => _inventorySlots.Length;

        [SerializeField] private int startingInventorySize = 25;
        private InventorySlot[] _inventorySlots;

        protected void Awake()
        {
            InitializeInventorySlots();
        }

        public bool AddItem(ItemData itemData)
        {
            if (itemData is StackableItemData)
                return AddStackableItem(itemData as StackableItemData);
            else
                return AddNonStackableItem(itemData);
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

            DecreaseStackSize(inventorySlot);
        }

        // public void Equip(InventorySlot inventorySlot)
        // {
        //     if (inventorySlot.ItemData == null)
        //         return;
        //     
        //     var item = Instantiate(inventorySlot.ItemData.prefab, transform.position, Quaternion.identity);
        //     item.GetComponent<Item>().Equip(transform);
        //     
        //     DecreaseStackSize(inventorySlot);
        // }

        public bool DragItem(InventorySlot from, InventorySlot to)
        {
            if (from.ItemData is StackableItemData && to.ItemData is StackableItemData)
                return DragStackableItem(from, to);
            else
                return DragNonStackableItem(from, to);
        }

        private bool AddStackableItem(StackableItemData itemData)
        {
            foreach (var item in _inventorySlots)
            {
                if (item.ItemData != itemData) 
                    continue;

                if (item.Amount >= itemData.maxStackSize) 
                    continue;
                
                item.IncrementStackSize(1);
                
                return true;
            }

            return TryAddItem(itemData);
        }

        private bool AddNonStackableItem(ItemData itemData)
        {
            return TryAddItem(itemData);
        }
        
        private void DecreaseStackSize(InventorySlot inventorySlot)
        {
            inventorySlot.IncrementStackSize(-1);

            if (inventorySlot.Amount == 0)
                inventorySlot.SetItem(null, 0);
        }

        private void InitializeInventorySlots()
        {
            _inventorySlots = new InventorySlot[startingInventorySize];

            for (int i = 0; i < startingInventorySize; i++)
            {
                _inventorySlots[i] = new InventorySlot(this, null, 0);
            }
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

        private bool DragStackableItem(InventorySlot from, InventorySlot to)
        {
            if (!(from.ItemData is StackableItemData fromData 
                  && to.ItemData is StackableItemData toData))
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