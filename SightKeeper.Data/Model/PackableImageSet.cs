using MemoryPack;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model;

/// <summary>
/// MemoryPackable version of <see cref="ImageSet"/>
/// </summary>
[MemoryPackable]
internal partial class PackableImageSet
{
	public required string Name { get; init; }
	public required string Description { get; init; }
	public required IReadOnlyCollection<PackableImage> Images { get; init; }
}