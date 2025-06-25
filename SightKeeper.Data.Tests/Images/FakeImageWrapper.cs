using SightKeeper.Data.Images;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Saving;

internal sealed class FakeImageWrapper : ImageWrapper
{
	public Image Wrap(InMemoryImage image)
	{
		return image;
	}
}