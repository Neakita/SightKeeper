using System.Collections.Generic;
using System.Linq;
using MahApps.Metro.IconPacks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.UI.WPF.Views.Pages;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public sealed class HamburgerMenuVM : ReactiveObject
{
	private HamburgerMenuItem _selectedMenuItem;

	public HamburgerMenuVM(ModelsPage modelsPage)
	{
		MenuItems = new HamburgerMenuItem[]
		{
			new("Dashboard", new Dashboard(), PackIconBootstrapIconsKind.Grid1x2,
				PackIconBootstrapIconsKind.Grid1x2Fill),
			new("Models", modelsPage, PackIconBootstrapIconsKind.Grid,
				PackIconBootstrapIconsKind.GridFill)
		};

		_selectedMenuItem = MenuItems.First();
		_selectedMenuItem.IsSelected = true;
	}

	[Reactive] public bool IsExpanded { get; set; }
	
	public IEnumerable<HamburgerMenuItem> MenuItems { get; }

	public IEnumerable<HamburgerMenuItem> OptionsMenuItems { get; } =
		Enumerable.Empty<HamburgerMenuItem>();

	public HamburgerMenuItem SelectedMenuItem
	{
		get => _selectedMenuItem;
		set
		{
			_selectedMenuItem.IsSelected = false;
			_selectedMenuItem = value;
			_selectedMenuItem.IsSelected = true;
			this.RaisePropertyChanged();
		}
	}
}