using System.Collections.Generic;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public interface IHamburgerMenuVM
{
	bool IsExpanded { get; set; }
	IEnumerable<HamburgerMenuItem> MenuItems { get; }
	HamburgerMenuItem SelectedMenuItem { get; set; }
}