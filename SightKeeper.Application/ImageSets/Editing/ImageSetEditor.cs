using System.Reactive.Subjects;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Editing;

public sealed class ImageSetEditor
{
	public IObservable<ImageSet> Edited => _edited;

	public void EditLibrary(ImageSet library, ImageSetData data)
	{
		library.Name = data.Name;
		library.Description = data.Description;
	}

	private readonly Subject<ImageSet> _edited = new();
}