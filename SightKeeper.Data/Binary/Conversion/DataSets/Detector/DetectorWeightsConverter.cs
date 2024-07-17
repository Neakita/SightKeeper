using System.Collections.Immutable;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Detector;

internal sealed class DetectorWeightsConverter
{
	public DetectorWeightsConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	internal ImmutableArray<SerializableWeightsWithTags> Convert(IEnumerable<DetectorWeights> weights, ConversionSession session)
	{
		return weights.Select(w => Convert(w, session)).ToImmutableArray();
	}

	private readonly FileSystemWeightsDataAccess _weightsDataAccess;

	private SerializableWeightsWithTags Convert(DetectorWeights weights, ConversionSession session)
	{
		return new SerializableWeightsWithTags(
			_weightsDataAccess.GetId(weights),
			weights,
			weights.Tags.Select(tag => session.Tags[tag]).ToImmutableArray());
	}
}