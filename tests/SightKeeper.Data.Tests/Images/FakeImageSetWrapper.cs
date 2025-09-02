using SightKeeper.Data.ImageSets;

namespace SightKeeper.Data.Tests.Images;

internal sealed class FakeImageSetWrapper : ImageSetWrapper
{
	public StorableImageSet Wrap(StorableImageSet set)
	{
		return set;
	}
}