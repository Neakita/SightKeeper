using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets;

public interface ReadOnlyDataSet<out TAsset>
{
	IEnumerable<ReadOnlyTag> Tags { get; }
	IEnumerable<TAsset> Assets { get; }
}