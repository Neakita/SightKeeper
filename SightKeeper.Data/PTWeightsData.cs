using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data;

internal sealed class PTWeightsData : DbWeightsData
{
	internal PTWeightsData(WeightsData data, Weights weights) : base(data, weights)
	{
	}

	private PTWeightsData()
	{
	}
}