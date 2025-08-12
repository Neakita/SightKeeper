using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal sealed class StorableItemsAssetExtension<TItem>(ItemsAsset<TItem> inner, StorableItemsAsset<TItem> innerExtended) : StorableItemsAsset<TItem>
{
	public StorableImage Image => (StorableImage)inner.Image;

	public StorableItemsAsset<TItem> Innermost => innerExtended.Innermost;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set => inner.Usage = value;
	}

	public IReadOnlyList<TItem> Items => inner.Items;

	public TItem MakeItem(StorableTag tag)
	{
		return inner.MakeItem(tag);
	}

	public void DeleteItemAt(int index)
	{
		inner.DeleteItemAt(index);
	}

	public void ClearItems()
	{
		inner.ClearItems();
	}
}