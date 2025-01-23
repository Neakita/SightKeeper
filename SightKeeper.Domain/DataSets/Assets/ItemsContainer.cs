namespace SightKeeper.Domain.DataSets.Assets;

public interface ItemsContainer
{
	IReadOnlyCollection<BoundedItem> Items { get; }
}