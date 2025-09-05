using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal interface WeightsWrapper
{
	WeightsData Wrap(WeightsData weights);
}