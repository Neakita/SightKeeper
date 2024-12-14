using MemoryPack;

namespace SightKeeper.Data.Model.DataSets.Weights;

[MemoryPackable]
internal sealed partial class PackablePoserWeights : PackableWeights
{
	public required IReadOnlyDictionary<byte, IReadOnlyCollection<byte>> TagsIndexes { get; init; }
}