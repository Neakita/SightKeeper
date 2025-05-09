using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Assets.Items;

public abstract class ItemsAsset<TItem> : ItemsAsset where TItem : BoundedItem
{
	public override IReadOnlyCollection<TItem> Items => _items.AsReadOnly();

	public sealed override TItem MakeItem(Tag tag, Bounding bounding)
	{
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(_tagsOwner, tag);
		var item = CreateItem(tag, bounding);
		_items.Add(item);
		return item;
	}

	public void DeleteItem(TItem item)
	{
		var isRemoved = _items.Remove(item);
		if (!isRemoved)
			throw new ArgumentException("Specified item was not found and therefore not deleted", nameof(item));
	}

	public void ClearItems()
	{
		_items.Clear();
	}

	protected ItemsAsset(TagsContainer<Tag> tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	protected abstract TItem CreateItem(Tag tag, Bounding bounding);

	private readonly List<TItem> _items = new();
	private readonly TagsContainer<Tag> _tagsOwner;
}