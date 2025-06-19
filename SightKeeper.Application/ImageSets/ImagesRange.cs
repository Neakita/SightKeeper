using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets;

public sealed class ImagesRange
{
	public required DomainImageSet Set { get; init; }
	public required IReadOnlyList<Image> Images { get; init; }
	public required Range Range { get; init; }
}