using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Images;

internal sealed class FakeImageWrapper : Wrapper<ManagedImage>
{
	public ManagedImage Wrap(ManagedImage image)
	{
		return image;
	}
}