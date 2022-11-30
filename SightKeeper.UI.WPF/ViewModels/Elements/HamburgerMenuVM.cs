using System.Collections.Generic;
using System.Linq;
using MahApps.Metro.IconPacks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.UI.WPF.Abstract;
using SightKeeper.UI.WPF.Views.Pages;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public sealed class HamburgerMenuVM : ReactiveObject, IHamburgerMenuVM
{
	public IEnumerable<IContentInclusiveMenuItem> MenuItems { get; } = new IContentInclusiveMenuItem[]
	{
		new HamburgerContentMenuItem("Dashboard", new Dashboard(), PackIconBootstrapIconsKind.Grid1x2, PackIconBootstrapIconsKind.Grid1x2Fill),
	};
	
	public IEnumerable<IContentInclusiveMenuItem> OptionsMenuItems { get; } = Enumerable.Empty<IContentInclusiveMenuItem>();

	[Reactive] public IContentInclusive SelectedMenuItem { get; set; }


	public HamburgerMenuVM() => SelectedMenuItem = MenuItems.First();
}
