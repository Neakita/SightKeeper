using SightKeeper.Application;
using SightKeeper.Application.ImageSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class LockingImageSetDeleter : ImageSetDeleter
{
	[Tag(typeof(AppData))] public required Lock AppDataLock { get; init; }

	public override void Delete(ImageSet set)
	{
		lock (AppDataLock)
			base.Delete(set);
	}
}