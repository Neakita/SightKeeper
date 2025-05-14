using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Annotation;

public interface AssetsDeleter
{
	void DeleteAsset(AssetsOwner<Asset> assetsOwner, Image image);
}