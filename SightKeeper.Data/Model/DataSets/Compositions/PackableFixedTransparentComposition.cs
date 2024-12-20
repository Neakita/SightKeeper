using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Data.Model.DataSets.Compositions;

/// <summary>
/// MemoryPackable version of <see cref="FixedTransparentImageComposition"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableFixedTransparentComposition : PackableComposition
{
	public required ImmutableArray<float> Opacities { get; init; }
}