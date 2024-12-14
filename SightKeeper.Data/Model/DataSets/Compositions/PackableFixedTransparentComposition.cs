using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Data.Binary.Model.DataSets.Compositions;

/// <summary>
/// MemoryPackable version of <see cref="FixedTransparentImageComposition"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableFixedTransparentComposition : PackableComposition
{
	public required ImmutableArray<float> Opacities { get; init; }
}