using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class StorableWeightsWrapper : Wrapper<WeightsData>
{
	public WeightsData Wrap(WeightsData weights)
	{
		return weights;
	}
}