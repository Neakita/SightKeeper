using System.Collections.Immutable;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DWeightsLibrary : WeightsLibrary<Poser2DWeights>
{
	public override Poser2DDataSet DataSet { get; }

	internal Poser2DWeightsLibrary(Poser2DDataSet dataSet)
	{
		DataSet = dataSet;
	}

	internal Poser2DWeights CreateWeights(
		ModelSize modelSize,
		WeightsMetrics metrics,
		ImmutableDictionary<Poser2DTag, ImmutableHashSet<KeyPointTag2D>> tags)
	{
		Poser2DWeights weights = new(modelSize, metrics, tags, this);
		AddWeights(weights);
		return weights;
	}
}