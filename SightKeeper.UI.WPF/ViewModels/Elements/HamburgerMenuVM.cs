using System.Collections.Generic;
using System.Linq;
using MahApps.Metro.IconPacks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.UI.WPF.Views.Pages;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public sealed class HamburgerMenuVM : ReactiveObject
{
	public HamburgerMenuVM(ModelsPage modelsPage)
	{
		MenuItems = new HamburgerMenuItem[]
		{
			new("Dashboard", new Dashboard(), PackIconBootstrapIconsKind.Grid1x2,
				PackIconBootstrapIconsKind.Grid1x2Fill),
			new("Models", modelsPage, PackIconBootstrapIconsKind.Grid,
				PackIconBootstrapIconsKind.GridFill)
		};
		SelectedMenuItem = MenuItems.First();
	}

	[Reactive] public bool IsExpanded { get; set; }
	
	public IEnumerable<HamburgerMenuItem> MenuItems { get; }

	[Reactive] public HamburgerMenuItem SelectedMenuItem { get; set; }
}