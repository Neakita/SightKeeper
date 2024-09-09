using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Model.DataSets.Weights;

/// <summary>
/// MemoryPackable version of <see cref="PlainWeights{TTag}"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePlainWeights : PackableWeights
{
	public ImmutableArray<byte> TagIds { get; }

	public PackablePlainWeights(
		ushort id,
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		ImmutableArray<byte> tagIds)
		: base(id, creationDate, modelSize, metrics, resolution)
	{
		TagIds = tagIds;
	}
}