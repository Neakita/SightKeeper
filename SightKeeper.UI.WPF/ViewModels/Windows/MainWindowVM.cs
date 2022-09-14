using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.UI.WPF.Views.Pages;

namespace SightKeeper.UI.WPF.ViewModels.Windows;

public class MainWindowVM : ReactiveObject
{
	public static MainWindowVM Current { get; private set; } = null!;
	
	#region SideMenu

	public SideMenuItemVM[] SideMenuItems { get; } =
	{
		new("Dashboard", new DashboardPage()),
		new("Annotator", new AnnotatorPage()),
		new("Models", new ModelsPage()),
		new("Settings", new SettingsPage())
	};

	[Reactive] public SideMenuItemVM SelectedSideMenuItem { get; set; } = null!;

	private void InitializeSideMenu()
	{
		if (!SideMenuItems.Any()) throw new InvalidOperationException("SideMenuItems array is empty, add side menus in code");
		SelectedSideMenuItem = SideMenuItems.First();
	}

	#endregion

	#region Popup

	private readonly ObservableCollection<UserControl> _popups = new();

	[ObservableAsProperty] public UserControl? PopupToShow { get; } = null;
	[ObservableAsProperty] public Visibility PopupVisibility { get; } = Visibility.Collapsed;
	
	public void ShowPopup(UserControl userControl) => _popups.Add(userControl);
	public void RemovePopup(UserControl userControl) => _popups.Remove(userControl);

	private void InitializePopup()
	{
		_popups.ToObservableChangeSet()
			.ToCollection()
			.Select(popups => popups.LastOrDefault())
			.ToPropertyEx(this, vm => vm.PopupToShow);

		this.WhenAnyValue(vm => vm.PopupToShow)
			.Select(popup => popup != null ? Visibility.Visible : Visibility.Collapsed)
			.ToPropertyEx(this, vm => vm.PopupVisibility);
	}

	#endregion
	
	public MainWindowVM()
	{
		InitializeSideMenu();
		InitializePopup();
		Current = this;
	}
}
