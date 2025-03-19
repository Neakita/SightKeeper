using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Editing;

public interface ImageSetEditor
{
	IObservable<ImageSet> Edited { get; }

	void EditLibrary(ImageSet set, ImageSetData data);
}