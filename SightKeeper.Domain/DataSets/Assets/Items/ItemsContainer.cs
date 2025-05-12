namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface ItemsContainer<out TItem>
{
	IReadOnlyList<TItem> Items { get; }
}