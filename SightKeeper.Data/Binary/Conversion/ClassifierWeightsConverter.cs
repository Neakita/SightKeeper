using System.Collections.Immutable;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class ClassifierWeightsConverter
{
	public ClassifierWeightsConverter(FileSystemClassifierWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	public ImmutableArray<SerializableWeightsWithTags> Convert(IEnumerable<ClassifierWeights> weights, ConversionSession session)
	{
		return weights.Select(w => Convert(w, session)).ToImmutableArray();
	}

	private readonly FileSystemClassifierWeightsDataAccess _weightsDataAccess;

	private SerializableWeightsWithTags Convert(ClassifierWeights weights, ConversionSession session)
	{
		return new SerializableWeightsWithTags(
			_weightsDataAccess.GetId(weights),
			weights,
			weights.Tags.Select(tag => session.Tags[tag]).ToImmutableArray());
	}
}