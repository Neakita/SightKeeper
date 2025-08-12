namespace SightKeeper.Data.DataSets.Weights;

internal interface WeightsWrapper
{
	StorableWeights Wrap(StorableWeights weights);
}