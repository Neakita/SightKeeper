using SightKeeper.Application;
using SightKeeper.Application.ImageSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataImageSetDeleter : ImageSetDeleter
{
	[Tag(typeof(AppData))] public required Lock AppDataLock { get; init; }
	public required ChangeListener ChangeListener { get; init; }

	public override void Delete(ImageSet set)
	{
		lock (AppDataLock)
			base.Delete(set);
		ChangeListener.SetDataChanged();
	}
}