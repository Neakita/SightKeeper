using System.Reactive.Subjects;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenshotsLibraries.Editing;

public sealed class ScreenshotsLibraryEditor
{
	public IObservable<ImageSet> Edited => _edited;

	public void EditLibrary(ImageSet library, ScreenshotsLibraryData data)
	{
		library.Name = data.Name;
		library.Description = data.Description;
	}

	private readonly Subject<ImageSet> _edited = new();
}