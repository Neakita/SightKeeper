using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training.Data;

public interface TrainData<out TAsset>
{
	IEnumerable<ReadOnlyTag> Tags { get; }
	IEnumerable<TAsset> Assets { get; }
}