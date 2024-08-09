using Material.Icons;

namespace SightKeeper.Avalonia.ViewModels.Elements;

internal sealed class TabItem : ViewModel
{
	public MaterialIconKind IconKind { get; }
	public string Header { get; }
	public ViewModel ViewModel { get; }
	
	public TabItem(MaterialIconKind iconKind, string header, ViewModel viewModel)
	{
		IconKind = iconKind;
		Header = header;
		ViewModel = viewModel;
	}
}