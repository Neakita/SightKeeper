using MemoryPack;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Data.Model;

/// <summary>
/// MemoryPackable version of <see cref="ScreenshotsLibrary"/>
/// </summary>
[MemoryPackable]
internal partial class PackableScreenshotsLibrary
{
	public required string Name { get; init; }
	public required string Description { get; init; }
	public required IReadOnlyCollection<PackableScreenshot> Screenshots { get; init; }
}