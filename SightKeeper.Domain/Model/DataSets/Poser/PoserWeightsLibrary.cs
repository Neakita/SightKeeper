using System.Collections.Immutable;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserWeightsLibrary : WeightsLibrary<PoserWeights>
{
	public override PoserDataSet DataSet { get; }

	internal PoserWeightsLibrary(PoserDataSet dataSet)
	{
		DataSet = dataSet;
	}

	internal PoserWeights CreateWeights(
		ModelSize modelSize,
		WeightsMetrics metrics,
		ImmutableDictionary<PoserTag, ImmutableHashSet<KeyPointTag>> tags)
	{
		PoserWeights weights = new(modelSize, metrics, tags, this);
		AddWeights(weights);
		return weights;
	}
}