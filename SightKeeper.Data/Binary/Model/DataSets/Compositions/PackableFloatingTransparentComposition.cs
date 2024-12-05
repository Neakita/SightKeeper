using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Model.DataSets.Compositions;

/// <summary>
/// MemoryPackable version of <see cref="FloatingTransparentComposition"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableFloatingTransparentComposition : PackableComposition
{
	public required TimeSpan SeriesDuration { get; init; }
	public required float PrimaryOpacity { get; init; }
	public required float MinimumOpacity { get; init; }
}