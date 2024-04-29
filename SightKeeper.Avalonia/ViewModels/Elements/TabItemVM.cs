using Material.Icons;
using Material.Icons.Avalonia;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class TabItem : ViewModel
{
	public object Icon { get; }
	public MaterialIconKind IconKind { get; }
	public string Header { get; }
	public ViewModel ViewModel { get; }
	
	public TabItem(MaterialIconKind iconKind, string header, ViewModel viewModel)
	{
		IconKind = iconKind;
		Icon = new MaterialIcon { Kind = iconKind };
		Header = header;
		ViewModel = viewModel;
	}
}
