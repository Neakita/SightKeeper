namespace SightKeeper.Application.Training.Data;

public interface ItemsAssetData<out TItem> : AssetData
{
	IEnumerable<TItem> Items { get; }
}