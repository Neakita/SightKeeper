namespace SightKeeper.Data.DataSets.Weights;

internal sealed class StorableWeightsWrapper : WeightsWrapper
{
	public StorableWeights Wrap(StorableWeights weights)
	{
		return weights;
	}
}