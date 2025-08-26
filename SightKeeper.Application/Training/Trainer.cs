using SightKeeper.Application.Training.Data;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Application.Training;

public interface Trainer<in TAsset>
{
	Task<Weights> TrainAsync(TrainData<TAsset> data, CancellationToken cancellationToken);
}