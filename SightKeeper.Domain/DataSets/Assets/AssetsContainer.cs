using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsContainer<out TAsset>
{
	TAsset? GetOptionalAsset(Screenshot screenshot);
}