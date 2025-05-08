using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Annotation;

public interface AssetDeleter
{
	void DeleteAsset(AssetsLibrary library, Image image);
}