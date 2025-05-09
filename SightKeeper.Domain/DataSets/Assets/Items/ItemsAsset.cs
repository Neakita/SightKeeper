using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Assets.Items;

public abstract class ItemsAsset : Asset, ItemsOwner
{
	public abstract IReadOnlyCollection<BoundedItem> Items { get; }

	public abstract BoundedItem MakeItem(Tag tag, Bounding bounding);
}