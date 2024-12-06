using SightKeeper.Data.Binary.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser;

internal sealed class PackablePoserWeights : PackableWeights
{
	public required IReadOnlyDictionary<byte, IReadOnlyCollection<byte>> TagsIndexes { get; init; }
}