using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Annotation;

public interface BoundingAnnotator
{
	BoundedItem CreateItem(AssetsMaker<ItemsCreator> assetsLibrary, Screenshot screenshot, Tag tag, Bounding bounding);
}