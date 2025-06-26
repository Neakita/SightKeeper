using SightKeeper.Data.Images;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Images;

internal sealed class FakeImageSetWrapper : ImageSetWrapper
{
	public ImageSet Wrap(ImageSet set)
	{
		return set;
	}
}