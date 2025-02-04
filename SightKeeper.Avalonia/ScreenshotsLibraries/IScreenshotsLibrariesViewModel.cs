using System.Collections.Generic;
using System.Windows.Input;

namespace SightKeeper.Avalonia.ScreenshotsLibraries;

internal interface IScreenshotsLibrariesViewModel
{
	IReadOnlyCollection<ScreenshotsLibraryViewModel> ScreenshotsLibraries { get; }
	ICommand CreateScreenshotsLibraryCommand { get; }
	ICommand EditScreenshotsLibraryCommand { get; }
	ICommand DeleteScreenshotsLibraryCommand { get; }
}