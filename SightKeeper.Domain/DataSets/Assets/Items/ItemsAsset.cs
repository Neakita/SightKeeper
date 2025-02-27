namespace SightKeeper.Domain.DataSets.Assets.Items;

public abstract class ItemsAsset : Asset, ItemsContainer
{
	public abstract IReadOnlyCollection<BoundedItem> Items { get; }
}