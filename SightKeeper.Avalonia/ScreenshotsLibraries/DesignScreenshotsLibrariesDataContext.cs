using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.ScreenshotsLibraries;

internal class DesignScreenshotsLibrariesDataContext : ScreenshotsLibrariesDataContext
{
	public IReadOnlyCollection<ScreenshotsLibraryViewModel> ScreenshotsLibraries { get; } =
	[
		new(new ScreenshotsLibrary { Name = "PD2" }),
		new(new ScreenshotsLibrary { Name = "KF2" }),
		new(new ScreenshotsLibrary { Name = "Some dataset specific library" })
	];

	public ICommand CreateScreenshotsLibraryCommand { get; } = new RelayCommand(() => { });
	public ICommand EditScreenshotsLibraryCommand { get; } = new RelayCommand(() => { });
	public ICommand DeleteScreenshotsLibraryCommand { get; } = new RelayCommand(() => { });
}