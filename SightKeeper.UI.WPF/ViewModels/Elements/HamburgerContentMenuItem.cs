using System.Reactive.Linq;
using System.Windows.Controls;
using MahApps.Metro.IconPacks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.UI.WPF.Abstract;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public sealed class HamburgerContentMenuItem : ReactiveObject, IContentInclusiveMenuItem, ISelectable
{
	public string Label { get; }
	public ContentControl Content { get; }
	[ObservableAsProperty] public Control Icon { get; } = null!;
	[Reactive] public bool IsSelected { get; set; }


	public HamburgerContentMenuItem(string label, ContentControl content,
		PackIconBootstrapIconsKind deactivatedIconKind, PackIconBootstrapIconsKind activatedIconKind)
	{
		Label = label;
		Content = content;
		PackIconBase activatedIcon = CreateIcon(activatedIconKind);
		PackIconBase deactivatedIcon = CreateIcon(deactivatedIconKind);
		this.WhenAnyValue(menuItem => menuItem.IsSelected)
			.Select(isSelected => isSelected ? activatedIcon : deactivatedIcon)
			.ToPropertyEx(this, menuItem => menuItem.Icon);
	}


	private static PackIconBase CreateIcon(PackIconBootstrapIconsKind iconKind) => new PackIconBootstrapIcons {Kind = iconKind};
}
