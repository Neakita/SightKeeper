using System.Reactive.Subjects;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenshotsLibraries.Editing;

public sealed class ScreenshotsLibraryEditor
{
	public IObservable<ScreenshotsLibrary> Edited => _edited;

	public void EditLibrary(ScreenshotsLibrary library, ScreenshotsLibraryData data)
	{
		library.Name = data.Name;
		library.Description = data.Description;
	}

	private readonly Subject<ScreenshotsLibrary> _edited = new();
}