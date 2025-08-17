using SightKeeper.Application.Training.Assets.Distribution;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Application.Training;

public interface Trainer<in TDataSet>
{
	Task<Weights> TrainAsync(TDataSet dataSet, AssetsDistributionRequest assetsDistributionRequest, CancellationToken cancellationToken);
}