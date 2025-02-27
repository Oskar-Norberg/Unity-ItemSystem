using Project.ItemSystem;

namespace Project.InventorySystem
{
    public class InventoryItem
    {
        public ItemData itemData;
        public int amount;

        public InventoryItem(ItemData itemData, int amount)
        {
            this.itemData = itemData;
            this.amount = amount;
        }
    }
}