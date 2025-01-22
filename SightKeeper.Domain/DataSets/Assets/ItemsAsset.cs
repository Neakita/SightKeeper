namespace SightKeeper.Domain.DataSets.Assets;

public abstract class ItemsAsset : Asset
{
	public abstract IReadOnlyCollection<BoundedItem> Items { get; }
}