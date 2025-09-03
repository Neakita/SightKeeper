using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets;

public interface DataSet : ReadOnlyDataSet
{
	string Name { get; set; }
	string Description { get; set; }

	TagsOwner<Tag> TagsLibrary { get; }
	AssetsOwner<Asset> AssetsLibrary { get; }
	WeightsLibrary WeightsLibrary { get; }

	IEnumerable<ReadOnlyTag> ReadOnlyDataSet.Tags => TagsLibrary.Tags;
	IEnumerable<ReadOnlyAsset> ReadOnlyDataSet.Assets => AssetsLibrary.Assets;
}