using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class PoserWeights : Weights
{
	public required IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> Tags { get; init; }
}