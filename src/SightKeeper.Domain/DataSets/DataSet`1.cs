using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets;

public interface DataSet<out TAsset> : DataSet, ReadOnlyDataSet<TAsset>
{
	new AssetsOwner<TAsset> AssetsLibrary { get; }

	AssetsOwner<Asset> DataSet.AssetsLibrary => (AssetsOwner<Asset>)AssetsLibrary;
	IEnumerable<ReadOnlyTag> ReadOnlyDataSet.Tags => TagsLibrary.Tags;
	IEnumerable<TAsset> ReadOnlyDataSet<TAsset>.Assets => AssetsLibrary.Assets;
	IEnumerable<ReadOnlyAsset> ReadOnlyDataSet.Assets => (IEnumerable<ReadOnlyAsset>)AssetsLibrary.Assets;
}