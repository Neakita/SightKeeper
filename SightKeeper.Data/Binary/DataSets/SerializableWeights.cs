using FlakeId;
using MemoryPack;
using SightKeeper.Data.Binary.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableWeightsWithTags))]
[MemoryPackUnion(1, typeof(SerializablePoser2DWeights))]
internal abstract partial class SerializableWeights
{
	public Id Id { get; }
	public DateTime CreationDate { get; }
	public ModelSize Size { get; }
	public WeightsMetrics Metrics { get; }

	[MemoryPackConstructor]
	protected SerializableWeights(Id id, DateTime creationDate, ModelSize size, WeightsMetrics metrics)
	{
		Id = id;
		CreationDate = creationDate;
		Size = size;
		Metrics = metrics;
	}

	protected SerializableWeights(Id id, Weights weights)
	{
		Id = id;
		CreationDate = weights.CreationDate;
		Size = weights.Size;
		Metrics = weights.Metrics;
	}
}