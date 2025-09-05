using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets;

public interface DataSet<out TTag, out TAsset> : ReadOnlyDataSet<TTag, TAsset>
{
	string Name { get; set; }
	string Description { get; set; }

	TagsOwner<TTag> TagsLibrary { get; }
	AssetsOwner<TAsset> AssetsLibrary { get; }
	WeightsLibrary WeightsLibrary { get; }

	IEnumerable<TTag> ReadOnlyDataSet<TTag, TAsset>.Tags => TagsLibrary.Tags;
	IEnumerable<TAsset> ReadOnlyDataSet<TTag, TAsset>.Assets => AssetsLibrary.Assets;
}