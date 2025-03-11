namespace Project.ItemSystem.Components
{
    // TODO: Consider making this a class to use RequireComponent.
    // This should then have some abstract base classes such as usable, right-clickable, etc.
    public interface IItemComponent
    {
        public Item Item { get; }
    }
}
