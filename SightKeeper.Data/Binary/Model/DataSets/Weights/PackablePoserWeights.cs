using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Model.DataSets.Weights;

/// <summary>
/// MemoryPackable version of <see cref="Weights{TTag,TKeyPointTag}"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoserWeights : PackableWeights
{
	public ImmutableDictionary<byte, ImmutableArray<byte>> Tags { get; }

	public PackablePoserWeights(
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		ImmutableDictionary<byte, ImmutableArray<byte>> tags)
		: base(creationDate, modelSize, metrics, resolution)
	{
		Tags = tags;
	}
}