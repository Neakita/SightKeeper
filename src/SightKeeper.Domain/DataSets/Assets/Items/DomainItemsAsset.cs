using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets.Items;

public sealed class DomainItemsAsset<TItem>(TagsContainer<Tag> tagsOwner, ItemsAsset<TItem> inner) : ItemsAsset<TItem>
{
	public ManagedImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set => inner.Usage = value;
	}

	public IReadOnlyList<TItem> Items => inner.Items;

	public TItem MakeItem(Tag tag)
	{
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(tagsOwner, tag);
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