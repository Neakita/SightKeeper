using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
internal partial class WeightsWithTags : Weights
{
	public ImmutableArray<Id> Tags { get; }

	[MemoryPackConstructor]
	public WeightsWithTags(Id id, DateTime creationDate, ModelSize size, WeightsMetrics metrics, ImmutableArray<Id> tags) : base(id, creationDate, size, metrics)
	{
		Tags = tags;
	}

	public WeightsWithTags(Id id, Domain.Model.DataSets.Weights.Weights weights, ImmutableArray<Id> tags) : base(id, weights)
	{
		Tags = tags;
	}
}