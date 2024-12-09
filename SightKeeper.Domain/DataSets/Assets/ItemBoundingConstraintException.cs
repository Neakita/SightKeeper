namespace SightKeeper.Domain.DataSets.Assets;

public sealed class ItemBoundingConstraintException : Exception
{
	public AssetItem Item { get; }
	public Bounding Value { get; }

	public ItemBoundingConstraintException(string? message, AssetItem item, Bounding value) : base(message)
	{
		Item = item;
		Value = value;
	}
}