using System.Reactive.Subjects;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Editing;

public class ImageSetEditor
{
	public IObservable<ImageSet> Edited => _edited;

	public virtual void EditImageSet(ExistingImageSetData data)
	{
		var set = data.Set;
		set.Name = data.Name;
		set.Description = data.Description;
		_edited.OnNext(set);
	}

	private readonly Subject<ImageSet> _edited = new();
}