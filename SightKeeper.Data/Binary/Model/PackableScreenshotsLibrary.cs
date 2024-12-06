using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Model;

/// <summary>
/// MemoryPackable version of <see cref="ScreenshotsLibrary"/>
/// </summary>
[MemoryPackable]
internal partial class PackableScreenshotsLibrary
{
	public required string Name { get; init; }
	public required ImmutableArray<PackableScreenshot> Screenshots { get; init; }
}