using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
public partial class SerializableWeightsWithTags : SerializableWeights
{
	public ImmutableArray<Id> Tags { get; }

	[MemoryPackConstructor]
	public SerializableWeightsWithTags(Id id, DateTime creationDate, ModelSize size, WeightsMetrics metrics, ImmutableArray<Id> tags) : base(id, creationDate, size, metrics)
	{
		Tags = tags;
	}

	public SerializableWeightsWithTags(Id id, Weights weights, ImmutableArray<Id> tags) : base(id, weights)
	{
		Tags = tags;
	}
}