using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Poser2D;

[MemoryPackable]
internal sealed partial class SerializablePoser2DWeights : SerializableWeights
{
	public ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> Tags { get; }

	[MemoryPackConstructor]
	public SerializablePoser2DWeights(
		Id id,
		DateTime creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> tags)
		: base(id, creationDate, size, metrics)
	{
		Tags = tags;
	}

	public SerializablePoser2DWeights(Id id, Weights weights, ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> tags) : base(id, weights)
	{
		Tags = tags;
	}
}