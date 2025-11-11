using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.Training;

public interface Trainer<in TTag, in TAsset>
{
	Task TrainAsync(ReadOnlyDataSet<TTag, TAsset> data, CancellationToken cancellationToken);
}