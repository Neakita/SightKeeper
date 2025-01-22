using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.ScreenshotsLibraries;

namespace SightKeeper.Avalonia.Annotation;

public sealed partial class AnnotationTabViewModel : ViewModel
{
	public ScreenshotsViewModel Screenshots { get; }
	[ObservableProperty] public partial DrawerViewModel? Drawer { get; private set; }
	public SideBarViewModel SideBar { get; }

	internal AnnotationTabViewModel(
		ScreenshotsViewModel screenshots,
		SideBarViewModel sideBar,
		DrawerViewModelFactory drawerViewModelFactory)
	{
		_drawerViewModelFactory = drawerViewModelFactory;
		SideBar = sideBar;
		Screenshots = screenshots;
		SideBar.SelectedScreenshotsLibraryChanged.Subscribe(OnSelectedScreenshotsLibraryChanged);
		SideBar.SelectedDataSetChanged.Subscribe(OnSelectedDataSetChanged);
	}

	private readonly DrawerViewModelFactory _drawerViewModelFactory;

	private void OnSelectedScreenshotsLibraryChanged(ScreenshotsLibraryViewModel? value)
	{
		Screenshots.Library = value?.Value;
	}

	private void OnSelectedDataSetChanged(DataSetViewModel? dataSetViewModel)
	{
		Drawer = _drawerViewModelFactory.CreateDrawerViewModel(dataSetViewModel?.Value);
	}
}