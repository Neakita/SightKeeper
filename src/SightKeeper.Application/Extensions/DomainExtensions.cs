using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Extensions;

public static class DomainExtensions
{
	public static bool CanDelete(this ImageSet set)
	{
		return set.Images.All(image => image.Assets.Count == 0);
	}
}