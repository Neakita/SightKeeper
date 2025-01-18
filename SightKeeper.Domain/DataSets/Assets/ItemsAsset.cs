namespace SightKeeper.Domain.DataSets.Assets;

public abstract class ItemsAsset : Asset
{
	public abstract IReadOnlyCollection<AssetItem> Items { get; }
}