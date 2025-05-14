using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets.Items;

public abstract class AbstractItemsAsset<TItem> : ItemsAsset<TItem> where TItem : BoundedItem
{
	public required Image Image { get; init; }
	public AssetUsage Usage { get; set; }
	public IReadOnlyList<TItem> Items => _items.AsReadOnly();

	public TItem MakeItem(Tag tag)
	{
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(_tagsOwner, tag);
		var item = CreateItem(tag);
		_items.Add(item);
		return item;
	}

	public void DeleteItem(TItem item)
	{
		var isRemoved = _items.Remove(item);
		if (!isRemoved)
			throw new ArgumentException("Specified item was not found and therefore not deleted", nameof(item));
	}

	public void DeleteItemAt(int index)
	{
		_items.RemoveAt(index);
	}

	public void ClearItems()
	{
		_items.Clear();
	}

	protected AbstractItemsAsset(TagsContainer<Tag> tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	protected abstract TItem CreateItem(Tag tag);

	private readonly List<TItem> _items = new();
	private readonly TagsContainer<Tag> _tagsOwner;
}