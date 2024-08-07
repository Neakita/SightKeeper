using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.DataSets.Poser;

[MemoryPackable]
internal sealed partial class SerializablePoserWeights : SerializableWeights
{
	public ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> Tags { get; }

	[MemoryPackConstructor]
	public SerializablePoserWeights(
		Id id,
		DateTime creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> tags)
		: base(id, creationDate, size, metrics)
	{
		Tags = tags;
	}

	public SerializablePoserWeights(Id id, Weights weights, ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> tags) : base(id, weights)
	{
		Tags = tags;
	}
}