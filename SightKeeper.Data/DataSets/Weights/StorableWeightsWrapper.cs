namespace SightKeeper.Data.DataSets.Weights;

internal sealed class StorableWeightsWrapper : WeightsWrapper
{
	public Domain.DataSets.Weights.Weights Wrap(Domain.DataSets.Weights.Weights weights)
	{
		return weights;
	}
}