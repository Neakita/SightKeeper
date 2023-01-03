using System.Reactive.Linq;
using System.Windows.Controls;
using MahApps.Metro.IconPacks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public sealed class HamburgerMenuItem : ReactiveObject
{
	public HamburgerMenuItem(string label, Control content,
		PackIconBootstrapIconsKind deactivatedIconKind, PackIconBootstrapIconsKind activatedIconKind)
		: this(label, content, CreateIcon(deactivatedIconKind), CreateIcon(activatedIconKind))
	{
	}

	private HamburgerMenuItem(string label, Control content, Control deactivatedIcon, Control activatedIcon)
	{
		Label = label;
		Content = content;
		
		this.WhenAnyValue(menuItem => menuItem.IsSelected)
			.Select(isSelected => isSelected ? activatedIcon : deactivatedIcon)
			.ToPropertyEx(this, menuItem => menuItem.Icon);
	}

	public string Label { get; }
	public Control Content { get; }
	[ObservableAsProperty] public object Icon { get; } = null!;
	[Reactive] public bool IsSelected { get; set; }


	private static Control CreateIcon(PackIconBootstrapIconsKind iconKind) =>
		new PackIconBootstrapIcons {Kind = iconKind};
}