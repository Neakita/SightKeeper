using MemoryPack;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Data.Binary.Model.DataSets.Compositions;

/// <summary>
/// MemoryPackable version of <see cref="Composition"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(0, typeof(PackableFixedTransparentComposition))]
[MemoryPackUnion(1, typeof(PackableFloatingTransparentComposition))]
internal abstract partial class PackableComposition
{
	public required TimeSpan MaximumScreenshotsDelay { get; init; }
}