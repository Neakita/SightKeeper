namespace SightKeeper.UI.Abstract;

public interface IHamburgerMenuVM
{
	IEnumerable<IContentInclusiveMenuItem> MenuItems { get; }
	IEnumerable<IContentInclusiveMenuItem> OptionsMenuItems { get; }
	IContentInclusiveMenuItem SelectedMenuItem { get; set; }
}
