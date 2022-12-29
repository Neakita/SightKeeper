using ReactiveUI;
using SightKeeper.UI.WPF.ViewModels.Elements;

namespace SightKeeper.UI.WPF.ViewModels.Windows;

public sealed class MainWindowVM : ReactiveObject
{
	public MainWindowVM(HamburgerMenuVM hamburgerMenuVM) => HamburgerMenuVM = hamburgerMenuVM;

	public HamburgerMenuVM HamburgerMenuVM { get; }
}