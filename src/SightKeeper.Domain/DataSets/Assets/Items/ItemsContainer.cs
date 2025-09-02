namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface ItemsContainer<out TItem>
{
	IEnumerable<TItem> Items { get; }
}