using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Annotation;

public interface BoundingAnnotator
{
	AssetItem CreateItem(AssetsOwner<ItemsMaker<AssetItem>> assetsLibrary, Image image, DomainTag tag, Bounding bounding);
}