using System.Collections.Generic;
using System.Windows.Input;

namespace SightKeeper.Avalonia.Screenshots;

internal interface IScreenshotsLibrariesViewModel
{
	IReadOnlyCollection<ScreenshotsLibraryViewModel> ScreenshotsLibraries { get; }
	ICommand CreateScreenshotsLibraryCommand { get; }
	ICommand EditScreenshotsLibraryCommand { get; }
	ICommand DeleteScreenshotsLibraryCommand { get; }
}