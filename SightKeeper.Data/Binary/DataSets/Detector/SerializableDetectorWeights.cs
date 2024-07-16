using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Detector;

[MemoryPackable]
public partial class SerializableDetectorWeights : SerializableWeights
{
	public IReadOnlyCollection<Id> Tags { get; }

	[MemoryPackConstructor]
	public SerializableDetectorWeights(Id id, DateTime creationDate, ModelSize size, WeightsMetrics metrics, IReadOnlyCollection<Id> tags) : base(id, creationDate, size, metrics)
	{
		Tags = tags;
	}

	public SerializableDetectorWeights(Id id, Weights weights, IReadOnlyCollection<Id> tags) : base(id, weights)
	{
		Tags = tags;
	}
}