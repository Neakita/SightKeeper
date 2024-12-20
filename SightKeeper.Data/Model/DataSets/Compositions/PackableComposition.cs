using MemoryPack;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Data.Model.DataSets.Compositions;

/// <summary>
/// MemoryPackable version of <see cref="ImageComposition"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(0, typeof(PackableFixedTransparentComposition))]
[MemoryPackUnion(1, typeof(PackableFloatingTransparentComposition))]
internal abstract partial class PackableComposition
{
	public required TimeSpan MaximumScreenshotsDelay { get; init; }
}