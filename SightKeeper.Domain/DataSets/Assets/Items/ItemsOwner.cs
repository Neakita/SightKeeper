namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface ItemsOwner<out TItem> : ItemsMaker<TItem>, ItemsContainer<TItem>
{
	new IReadOnlyList<TItem> Items { get; }
	void DeleteItemAt(int index);
	void ClearItems();

	IEnumerable<TItem> ItemsContainer<TItem>.Items => Items;
}