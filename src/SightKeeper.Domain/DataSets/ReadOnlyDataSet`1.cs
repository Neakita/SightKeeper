using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Domain.DataSets;

public interface ReadOnlyDataSet<out TAsset> : ReadOnlyDataSet
{
	new IEnumerable<TAsset> Assets { get; }

	IEnumerable<ReadOnlyAsset> ReadOnlyDataSet.Assets => (IEnumerable<ReadOnlyAsset>)Assets;
}