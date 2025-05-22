using SightKeeper.Application.ImageSets.Editing;

namespace SightKeeper.Data.Services;

public sealed class AppDataImageSetEditor : ImageSetEditor
{
	public required Lock AppDataLock { get; init; }
	public required ChangeListener ChangeListener { get; init; }

	public override void EditImageSet(ExistingImageSetData data)
	{
		lock (AppDataLock)
			base.EditImageSet(data);
		ChangeListener.SetDataChanged();
	}
}