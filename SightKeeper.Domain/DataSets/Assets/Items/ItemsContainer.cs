namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface ItemsContainer
{
	IReadOnlyCollection<BoundedItem> Items { get; }
}