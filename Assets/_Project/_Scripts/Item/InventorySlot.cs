using Project.ItemSystem;

namespace Project.InventorySystem
{
    public class InventorySlot
    {
        public ItemData ItemData { get; private set; }
        public int Amount { get; private set; }
        
        public delegate void ItemSetEventHandler(InventorySlot inventorySlot);
        public event ItemSetEventHandler OnItemSet;

        public InventorySlot(ItemData itemData, int amount)
        {
            ItemData = itemData;
            Amount = amount;
        }
        
        public void SetItem(ItemData itemData, int amount)
        {
            ItemData = itemData;
            Amount = amount;
            
            OnItemSet?.Invoke(this);
        }
        
        public void IncreaseStackSize(int amount)
        {
            Amount += amount;
            
            OnItemSet?.Invoke(this);
        }
    }
}