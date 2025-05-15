using System.Reactive.Subjects;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Editing;

public class ImageSetEditor
{
	public IObservable<ImageSet> Edited => _edited;

	public virtual void EditImageSet(ImageSet set, ImageSetData data)
	{
		set.Name = data.Name;
		set.Description = data.Description;
	}

	private readonly Subject<ImageSet> _edited = new();
}