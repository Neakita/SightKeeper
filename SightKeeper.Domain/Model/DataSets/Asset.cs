namespace SightKeeper.Domain.Model.DataSets;

public abstract class Asset
{
	public AssetUsage Usage { get; set; } = AssetUsage.Any;
	public abstract Screenshot Screenshot { get; }
	public abstract AssetsLibrary Library { get; }
	public abstract DataSet DataSet { get; }
}