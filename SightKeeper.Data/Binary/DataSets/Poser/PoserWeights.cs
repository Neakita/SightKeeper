using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.DataSets.Poser;

[MemoryPackable]
internal sealed partial class PoserWeights : Weights
{
	public ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> Tags { get; }

	[MemoryPackConstructor]
	public PoserWeights(
		Id id,
		DateTime creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> tags)
		: base(id, creationDate, size, metrics)
	{
		Tags = tags;
	}

	public PoserWeights(Id id, Domain.Model.DataSets.Weights.Weights weights, ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> tags) : base(id, weights)
	{
		Tags = tags;
	}
}