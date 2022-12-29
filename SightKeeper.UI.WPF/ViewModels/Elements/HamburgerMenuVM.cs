using System.Collections.Generic;
using System.Linq;
using MahApps.Metro.IconPacks;
using ReactiveUI;
using SightKeeper.UI.WPF.Abstract;
using SightKeeper.UI.WPF.Views.Pages;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public sealed class HamburgerMenuVM : ReactiveObject, IHamburgerMenuVM
{
	private IContentInclusiveMenuItem _selectedMenuItem;


	public HamburgerMenuVM(ModelsPage modelsPage)
	{
		MenuItems = new IContentInclusiveMenuItem[]
		{
			new HamburgerContentMenuItem("Dashboard", new Dashboard(), PackIconBootstrapIconsKind.Grid1x2,
				PackIconBootstrapIconsKind.Grid1x2Fill),
			new HamburgerContentMenuItem("Models", modelsPage, PackIconBootstrapIconsKind.Grid,
				PackIconBootstrapIconsKind.GridFill)
		};

		_selectedMenuItem = MenuItems.First();
		_selectedMenuItem.IsSelected = true;
	}

	public IEnumerable<IContentInclusiveMenuItem> MenuItems { get; }

	public IEnumerable<IContentInclusiveMenuItem> OptionsMenuItems { get; } =
		Enumerable.Empty<IContentInclusiveMenuItem>();

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
}