namespace SightKeeper.Domain.Model.DataSets.Assets;

public abstract class Asset
{
	public AssetUsage Usage { get; set; } = AssetUsage.Any;
}