using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Images;

internal sealed class FakeImageWrapper : ImageWrapper
{
	public Image Wrap(InMemoryImage image)
	{
		return image;
	}
}