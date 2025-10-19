using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Images;

internal sealed class FakeImageSetWrapper : Wrapper<ImageSet>
{
	public ImageSet Wrap(ImageSet set)
	{
		return set;
	}
}