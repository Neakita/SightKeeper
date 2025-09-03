using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface Asset : ReadOnlyAsset
{
	new ManagedImage Image { get; }
	new AssetUsage Usage { get; set; }

	ImageData ReadOnlyAsset.Image => Image;
	AssetUsage ReadOnlyAsset.Usage => Usage;
}