using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Annotation;

public interface BoundingAnnotator
{
	AssetItem CreateItem(AssetsOwner<ItemsMaker<AssetItem>> assetsLibrary, DomainImage image, DomainTag tag, Bounding bounding);
}