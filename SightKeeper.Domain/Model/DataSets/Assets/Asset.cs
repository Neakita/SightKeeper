using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Assets;

public abstract class Asset
{
	public AssetUsage Usage { get; set; } = AssetUsage.Any;
	public abstract Screenshot Screenshot { get; }
	public abstract AssetsLibrary Library { get; }
	public abstract DataSet DataSet { get; }
}