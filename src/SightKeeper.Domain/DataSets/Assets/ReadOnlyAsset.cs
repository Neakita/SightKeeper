using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface ReadOnlyAsset
{
	ImageData Image { get; }
	AssetUsage Usage { get; }
}