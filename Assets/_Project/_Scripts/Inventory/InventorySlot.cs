using Project.ItemSystem;

namespace Project.InventorySystem
{
    public class InventorySlot
    {
        public ItemData ItemData { get; private set; }
        public int Amount { get; private set; }
        
        public delegate void ItemSetEventHandler(InventorySlot inventorySlot);
        public event ItemSetEventHandler OnItemSet;

        private Inventory _inventory;

        public InventorySlot(Inventory inventory, ItemData itemData, int amount)
        {
            _inventory = inventory;
            ItemData = itemData;
            Amount = amount;
        }
        
        public void SetItem(ItemData itemData, int amount)
        {
            ItemData = itemData;
            Amount = amount;
            
            OnItemSet?.Invoke(this);
        }
        
        public void IncrementStackSize(int amount)
        {
            Amount += amount;
            
            OnItemSet?.Invoke(this);
        }

        public void Drop()
        {
            if (ItemData)
                _inventory.DropItem(this);
        }
    }
}