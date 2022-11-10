using System.Reactive.Linq;
using MahApps.Metro.IconPacks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SightKeeper.UI.WPF.ViewModels;

public sealed class HamburgerMenuItemVM : ReactiveObject
{
	private static PackIconBootstrapIcons IconFactory(PackIconBootstrapIconsKind kind) => new() {Kind = kind};


	public string Label { get; }
	[Reactive] public bool IsSelected { get; set; } = false;
	[ObservableAsProperty] public PackIconBase Icon { get; } = null!;


	public HamburgerMenuItemVM(string label, PackIconBootstrapIconsKind activatedIconKind, PackIconBootstrapIconsKind? deactivatedIconKind = null)
	{
		Label = label;
		PackIconBootstrapIcons activatedIcon = IconFactory(activatedIconKind);
		PackIconBootstrapIcons? deactivatedIcon = deactivatedIconKind.HasValue ? IconFactory(deactivatedIconKind.Value) : null;
		this.WhenAnyValue(vm => vm.IsSelected)
			.Select(isSelected => isSelected ? activatedIcon : deactivatedIcon)
			.ToPropertyEx(this, vm => vm.Icon);
	}
}
