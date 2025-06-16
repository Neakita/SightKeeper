using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public interface PoserDataSet : DataSet
{
	new TagsOwner<PoserTag> TagsLibrary { get; }
	new AssetsOwner<PoserAsset<PoserItem>> AssetsLibrary { get; }

	TagsOwner<Tag> DataSet.TagsLibrary => TagsLibrary;
	AssetsOwner<Asset> DataSet.AssetsLibrary => AssetsLibrary;
}