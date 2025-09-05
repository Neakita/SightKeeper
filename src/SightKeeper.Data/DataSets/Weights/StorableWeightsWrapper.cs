using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class StorableWeightsWrapper : WeightsWrapper
{
	public WeightsData Wrap(WeightsData weights)
	{
		return weights;
	}
}