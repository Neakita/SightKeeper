using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data;

internal abstract class DbWeightsData
{
	public WeightsData Data { get; }
	public Weights Weights { get; }

	protected DbWeightsData(WeightsData data, Weights weights)
	{
		Data = data;
		Weights = weights;
	}

	protected DbWeightsData()
	{
		Data = null!;
		Weights = null!;
	}
}