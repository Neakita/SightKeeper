using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Screenshots;

internal sealed class ScreenshotsLibraryViewModel : ViewModel
{
	public ScreenshotsLibrary Value { get; }
	public string Name => Value.Name;
	public Screenshot? PreviewScreenshot => Value.Screenshots.RandomOrDefault();

	public ScreenshotsLibraryViewModel(ScreenshotsLibrary value)
	{
		Value = value;
	}
}