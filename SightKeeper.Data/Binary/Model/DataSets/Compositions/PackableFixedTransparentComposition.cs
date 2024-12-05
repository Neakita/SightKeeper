using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Model.DataSets.Compositions;

/// <summary>
/// MemoryPackable version of <see cref="FixedTransparentComposition"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableFixedTransparentComposition : PackableComposition
{
	public required ImmutableArray<float> Opacities { get; init; }
}