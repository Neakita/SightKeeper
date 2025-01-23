using System;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.ScreenshotsLibraries;

namespace SightKeeper.Avalonia.Annotation;

public sealed class AnnotationTabViewModel : ViewModel
{
	public ScreenshotsViewModel Screenshots { get; }
	public DrawerViewModel Drawer { get; }
	public SideBarViewModel SideBar { get; }

	internal AnnotationTabViewModel(
		ScreenshotsViewModel screenshots,
		DrawerViewModel drawer,
		SideBarViewModel sideBar)
	{
		SideBar = sideBar;
		Drawer = drawer;
		Screenshots = screenshots;
		SideBar.SelectedScreenshotsLibraryChanged.Subscribe(OnSelectedScreenshotsLibraryChanged);
	}

	private void OnSelectedScreenshotsLibraryChanged(ScreenshotsLibraryViewModel? value)
	{
		Screenshots.Library = value?.Value;
	}
}