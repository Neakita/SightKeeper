using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets;

public interface ReadOnlyDataSet
{
	IEnumerable<ReadOnlyTag> Tags { get; }
	IEnumerable<ReadOnlyAsset> Assets { get; }
}