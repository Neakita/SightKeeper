using System.Collections.Generic;
using System.Windows.Controls;

namespace SightKeeper.UI.WPF.Abstract;

public interface IHamburgerMenuVM
{
	IEnumerable<IContentInclusiveMenuItem> MenuItems { get; }
	IEnumerable<IContentInclusiveMenuItem> OptionsMenuItems { get; }
	IContentInclusive SelectedMenuItem { get; set; }
}
