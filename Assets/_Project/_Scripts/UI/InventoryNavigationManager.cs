namespace Project.InventorySystem.UI
{
    public class InventoryNavigationManager
    {
        private Inventory _inventory;
        
        private InventorySlot _dragFromSlot;
        
        public InventoryNavigationManager(Inventory inventory)
        {
            _inventory = inventory;
        }

        public void OnBeginDrag(InventorySlot inventorySlot)
        {
            _dragFromSlot = inventorySlot;
        }

        public void OnFinishDrag(InventorySlot inventorySlot)
        {
            if (_dragFromSlot == null || inventorySlot == null)
                return;
            
            if (_dragFromSlot == inventorySlot)
                return;
            
            _inventory.DragItem(_dragFromSlot, inventorySlot);
            _dragFromSlot = null;
        }
    }
}
