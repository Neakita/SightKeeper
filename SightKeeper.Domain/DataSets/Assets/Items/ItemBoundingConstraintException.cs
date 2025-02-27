namespace SightKeeper.Domain.DataSets.Assets.Items;

public sealed class ItemBoundingConstraintException : Exception
{
	public BoundedItem Item { get; }
	public Bounding Value { get; }

	public ItemBoundingConstraintException(string? message, BoundedItem item, Bounding value) : base(message)
	{
		Item = item;
		Value = value;
	}
}