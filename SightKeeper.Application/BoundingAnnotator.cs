using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application;

public interface BoundingAnnotator
{
	BoundedItem CreateItem(AssetsMaker<ItemsCreator> assetsLibrary, Screenshot screenshot, Tag tag, Bounding bounding);
}