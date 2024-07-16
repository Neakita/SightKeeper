using System.Collections.Immutable;
using SightKeeper.Data.Binary.DataSets.Detector;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary.Conversion;

public sealed class DetectorWeightsConverter
{
	public DetectorWeightsConverter(FileSystemDetectorWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	internal ImmutableArray<SerializableDetectorWeights> Convert(IEnumerable<DetectorWeights> weights, ConversionSession session)
	{
		return weights.Select(w => Convert(w, session)).ToImmutableArray();
	}

	private readonly FileSystemDetectorWeightsDataAccess _weightsDataAccess;

	private SerializableDetectorWeights Convert(DetectorWeights weights, ConversionSession session)
	{
		return new SerializableDetectorWeights(
			_weightsDataAccess.GetId(weights),
			weights,
			weights.Tags.Select(tag => session.Tags[tag]).ToImmutableArray());
	}
}