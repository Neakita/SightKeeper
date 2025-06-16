using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests;

internal static class Extensions
{
	public static DomainImage CreateImage(this DomainImageSet set)
	{
		return set.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
	}
}