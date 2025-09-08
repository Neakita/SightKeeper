namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface ItemsMaker<out TItem>
{
	TItem MakeItem();
}