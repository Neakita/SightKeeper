using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.ScreenshotsLibraries;

namespace SightKeeper.Avalonia.Annotation;

public sealed partial class AnnotationTabViewModel : ViewModel
{
	public ScreenshotsViewModel Screenshots { get; }
	[ObservableProperty] public partial DrawerViewModel? Drawer { get; private set; }
	public SideBarViewModel SideBar { get; }

	internal AnnotationTabViewModel(
		ScreenshotsViewModel screenshots,
		SideBarViewModel sideBar)
	{
		SideBar = sideBar;
		Screenshots = screenshots;
		SideBar.SelectedScreenshotsLibraryChanged.Subscribe(OnSelectedScreenshotsLibraryChanged);
	}

	private void OnSelectedScreenshotsLibraryChanged(ScreenshotsLibraryViewModel? value)
	{
		Screenshots.Library = value?.Value;
	}
}