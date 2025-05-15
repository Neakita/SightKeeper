using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataImageSetEditor : ImageSetEditor
{
	public required Lock AppDataLock { get; init; }
	public required ChangeListener ChangeListener { get; init; }

	public override void EditImageSet(ImageSet set, ImageSetData data)
	{
		lock (AppDataLock)
			base.EditImageSet(set, data);
		ChangeListener.SetDataChanged();
	}
}