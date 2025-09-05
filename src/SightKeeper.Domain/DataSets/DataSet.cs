using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets;

public interface DataSet<out TAsset> : ReadOnlyDataSet<TAsset>
{
	string Name { get; set; }
	string Description { get; set; }

	TagsOwner<Tag> TagsLibrary { get; }
	AssetsOwner<TAsset> AssetsLibrary { get; }
	WeightsLibrary WeightsLibrary { get; }

	IEnumerable<ReadOnlyTag> ReadOnlyDataSet<TAsset>.Tags => TagsLibrary.Tags;
	IEnumerable<TAsset> ReadOnlyDataSet<TAsset>.Assets => AssetsLibrary.Assets;
}