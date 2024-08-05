using System.Collections.Immutable;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DWeightsLibrary : WeightsLibrary<Poser3DWeights>
{
	public override Poser3DDataSet DataSet { get; }

	internal Poser3DWeightsLibrary(Poser3DDataSet dataSet)
	{
		DataSet = dataSet;
	}

	internal Poser3DWeights CreateWeights(
		ModelSize modelSize,
		WeightsMetrics metrics,
		ImmutableDictionary<Poser3DTag, ImmutableHashSet<KeyPointTag3D>> tags)
	{
		Poser3DWeights weights = new(modelSize, metrics, tags, this);
		AddWeights(weights);
		return weights;
	}
}