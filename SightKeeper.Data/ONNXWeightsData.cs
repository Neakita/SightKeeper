using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data;

internal sealed class ONNXWeightsData : DbWeightsData
{
	internal ONNXWeightsData(WeightsData data, Weights weights) : base(data, weights)
	{
	}

	private ONNXWeightsData()
	{
	}
}