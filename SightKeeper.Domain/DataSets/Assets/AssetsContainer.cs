using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsContainer<out TAsset>
{
	TAsset? GetOptionalAsset(Image image);
	bool Contains(Image image);
}