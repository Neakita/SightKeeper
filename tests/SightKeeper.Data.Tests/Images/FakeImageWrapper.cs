using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data.Tests.Images;

internal sealed class FakeImageWrapper : ImageWrapper
{
	public StorableImage Wrap(StorableImage image)
	{
		return image;
	}
}