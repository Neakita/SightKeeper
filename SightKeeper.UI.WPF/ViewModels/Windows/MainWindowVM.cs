using System.Reactive.Linq;
using System.Windows.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.UI.WPF.ViewModels.Elements;

namespace SightKeeper.UI.WPF.ViewModels.Windows;

public sealed class MainWindowVM : ReactiveObject
{
	public MainWindowVM(HamburgerMenuVM hamburgerMenuVM)
	{
		HamburgerMenuVM = hamburgerMenuVM;
		
		HamburgerMenuVM.WhenAnyValue(vm => vm.SelectedMenuItem)
			.Select(menuItem => menuItem.Content)
			.ToPropertyEx(this, vm => vm.Content);
	}

	public HamburgerMenuVM HamburgerMenuVM { get; }

	[ObservableAsProperty] public Control Content { get; } = null!;
}