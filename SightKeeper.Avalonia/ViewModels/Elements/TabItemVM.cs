using Material.Icons;
using Material.Icons.Avalonia;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class TabItem : ViewModel
{
	public object Icon { get; }
	public string Header { get; }
	public ViewModel ViewModel { get; }
	
	public TabItem(MaterialIconKind iconKind, string header, ViewModel viewModel)
	{
		Icon = new MaterialIcon {Kind = iconKind, Width = 20, Height = 20};
		Header = header;
		ViewModel = viewModel;
	}
}
