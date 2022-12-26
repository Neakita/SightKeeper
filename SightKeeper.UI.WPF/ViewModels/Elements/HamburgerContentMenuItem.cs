using System.Reactive.Linq;
using System.Windows.Controls;
using MahApps.Metro.IconPacks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.UI.WPF.Abstract;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public sealed class HamburgerContentMenuItem : ReactiveObject, IContentInclusiveMenuItem
{
	public string Label { get; }
	public object Content { get; }
	[ObservableAsProperty] public object Icon { get; } = null!;
	[Reactive] public bool IsSelected { get; set; }


	public HamburgerContentMenuItem(string label, ContentControl content, PackIconBootstrapIconsKind deactivatedIconKind, PackIconBootstrapIconsKind activatedIconKind) 
		: this(label, content, CreateIcon(deactivatedIconKind), CreateIcon(activatedIconKind)) { }

	private HamburgerContentMenuItem(string label, ContentControl content,
		Control deactivatedIconKind, Control activatedIconKind)
	{
		Label = label;
		Content = content;
		this.WhenAnyValue(menuItem => menuItem.IsSelected)
			.Select(isSelected => isSelected ? activatedIconKind : deactivatedIconKind)
			.ToPropertyEx(this, menuItem => menuItem.Icon);
	}


	private static Control CreateIcon(PackIconBootstrapIconsKind iconKind) => new PackIconBootstrapIcons {Kind = iconKind};
}
