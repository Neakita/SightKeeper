using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public abstract class Asset
{
	public required Image Image { get; init; }
	public AssetUsage Usage { get; set; } = AssetUsage.Any;
}