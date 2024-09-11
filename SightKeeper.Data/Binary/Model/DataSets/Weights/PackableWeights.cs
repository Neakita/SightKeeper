using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Model.DataSets.Weights;

/// <summary>
/// MemoryPackable version of <see cref="Weights"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableWeights
{
	public ushort Id { get; }
	public DateTime CreationDate { get; }
	public ModelSize ModelSize { get; }
	public WeightsMetrics Metrics { get; }
	public Vector2<ushort> Resolution { get; }
	public ImmutableArray<byte> TagIds { get; }

	public PackableWeights(
		ushort id,
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		ImmutableArray<byte> tagIds)
	{
		Id = id;
		CreationDate = creationDate;
		ModelSize = modelSize;
		Metrics = metrics;
		Resolution = resolution;
		TagIds = tagIds;
	}
}