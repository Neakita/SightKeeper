using System.Collections.Generic;
using System.Linq;
using MahApps.Metro.IconPacks;
using ReactiveUI;
using SightKeeper.UI.WPF.Abstract;
using SightKeeper.UI.WPF.ViewModels.Pages;
using SightKeeper.UI.WPF.Views.Pages;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public sealed class HamburgerMenuVM : ReactiveObject, IHamburgerMenuVM
{
	public IEnumerable<IContentInclusiveMenuItem> MenuItems { get; }
	
	public IEnumerable<IContentInclusiveMenuItem> OptionsMenuItems { get; } = Enumerable.Empty<IContentInclusiveMenuItem>();
	
	public IContentInclusiveMenuItem SelectedMenuItem
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


	public HamburgerMenuVM(ModelsPage modelsPage)
	{
		MenuItems = new IContentInclusiveMenuItem[]
		{
			new HamburgerContentMenuItem("Dashboard", new Dashboard(), PackIconBootstrapIconsKind.Grid1x2, PackIconBootstrapIconsKind.Grid1x2Fill),
			new HamburgerContentMenuItem("Models", modelsPage, PackIconBootstrapIconsKind.Grid, PackIconBootstrapIconsKind.GridFill)
		};
		
		_selectedMenuItem = MenuItems.First();
		_selectedMenuItem.IsSelected = true;
	}


	private IContentInclusiveMenuItem _selectedMenuItem;
}
