namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface ItemsOwner<out TItem> : ItemsMaker<TItem>, ItemsContainer<TItem>
{
	void DeleteItemAt(int index);
	void ClearItems();
}