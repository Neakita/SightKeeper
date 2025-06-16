using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Annotation;

public interface AssetsDeleter
{
	void DeleteAsset(AssetsOwner<Asset> assetsOwner, DomainImage image);
}