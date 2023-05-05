using Avalonia.Controls;
using Material.Icons;
using Material.Icons.Avalonia;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class TabItem : ViewModel
{
	public object Icon { get; }
	public string Header { get; }
	public Control Content { get; }
	
	public TabItem(MaterialIconKind iconKind, string header, Control content)
	{
		Icon = new MaterialIcon {Kind = iconKind, Width = 20, Height = 20};
		Header = header;
		Content = content;
	}
}
