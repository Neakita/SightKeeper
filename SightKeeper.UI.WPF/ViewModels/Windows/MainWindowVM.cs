using System;
using System.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.UI.WPF.Views.Pages;

namespace SightKeeper.UI.WPF.ViewModels.Windows;

public class MainWindowVM : ReactiveObject
{
	#region SideMenu

	public SideMenuItemVM[] SideMenuItems { get; } =
	{
		new("Dashboard", new DashboardPage()),
		new("Annotator", new AnnotatorPage())
	};

	[Reactive] public SideMenuItemVM SelectedSideMenuItem { get; set; } = null!;

	private void InitializeSideMenu()
	{
		if (!SideMenuItems.Any()) throw new InvalidOperationException("SideMenuItems array is empty, add side menus in code");
		SelectedSideMenuItem = SideMenuItems.First();
	}

	#endregion
	
	
	public MainWindowVM()
	{
		InitializeSideMenu();
	}
}
