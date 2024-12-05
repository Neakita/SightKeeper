using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Model.DataSets.Weights;

/// <summary>
/// MemoryPackable version of <see cref="PlainWeights"/>
/// </summary>
[MemoryPackable]
internal partial class PackablePlainWeights : PackableWeights
{
	public required ImmutableArray<byte> TagIds { get; init; }
}