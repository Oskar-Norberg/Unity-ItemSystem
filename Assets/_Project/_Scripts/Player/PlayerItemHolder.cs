using Project.InteractableSystem;

namespace Project.PlayerCharacter.Item
{
    public class PlayerItemHolder : ItemHolder
    {
        private void OnSubmit()
        {
            if (!CurrentItem)
                return;
            
            CurrentItem.Item.Use(transform);
        }

        private void OnRightClick()
        {
            Drop();
        }
    }
}