using MemoryPack;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Data.Model.DataSets.Compositions;

/// <summary>
/// MemoryPackable version of <see cref="FloatingTransparentImageComposition"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableFloatingTransparentComposition : PackableComposition
{
	public required TimeSpan SeriesDuration { get; init; }
	public required float PrimaryOpacity { get; init; }
	public required float MinimumOpacity { get; init; }
}