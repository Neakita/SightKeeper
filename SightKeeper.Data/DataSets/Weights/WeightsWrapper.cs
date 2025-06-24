namespace SightKeeper.Data.DataSets.Weights;

internal interface WeightsWrapper
{
	Domain.DataSets.Weights.Weights Wrap(Domain.DataSets.Weights.Weights weights);
}