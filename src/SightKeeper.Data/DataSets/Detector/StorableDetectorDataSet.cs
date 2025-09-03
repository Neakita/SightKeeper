using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Detector;

public interface StorableDetectorDataSet : DetectorDataSet
{
	new StorableTagsOwner<StorableTag> TagsLibrary { get; }
	new StorableAssetsOwner<StorableItemsAsset<StorableDetectorItem>> AssetsLibrary { get; }
	new StorableWeightsLibrary WeightsLibrary { get; }
	StorableDetectorDataSet Innermost { get; }

	TagsOwner<Tag> DataSet.TagsLibrary => TagsLibrary;
	AssetsOwner<ItemsAsset<DetectorItem>> DataSet<ItemsAsset<DetectorItem>>.AssetsLibrary => AssetsLibrary;
	WeightsLibrary DataSet.WeightsLibrary => WeightsLibrary;
}