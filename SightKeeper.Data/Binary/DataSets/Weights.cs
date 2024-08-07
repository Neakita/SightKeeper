using FlakeId;
using MemoryPack;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
[MemoryPackUnion(0, typeof(WeightsWithTags))]
[MemoryPackUnion(1, typeof(PoserWeights))]
internal abstract partial class Weights
{
	public Id Id { get; }
	public DateTime CreationDate { get; }
	public ModelSize Size { get; }
	public WeightsMetrics Metrics { get; }

	[MemoryPackConstructor]
	protected Weights(Id id, DateTime creationDate, ModelSize size, WeightsMetrics metrics)
	{
		Id = id;
		CreationDate = creationDate;
		Size = size;
		Metrics = metrics;
	}

	protected Weights(Id id, Domain.Model.DataSets.Weights.Weights weights)
	{
		Id = id;
		CreationDate = weights.CreationDate;
		Size = weights.Size;
		Metrics = weights.Metrics;
	}
}