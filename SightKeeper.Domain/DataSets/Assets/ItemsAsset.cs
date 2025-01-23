namespace SightKeeper.Domain.DataSets.Assets;

public abstract class ItemsAsset : Asset, ItemsContainer
{
	public abstract IReadOnlyCollection<BoundedItem> Items { get; }
}