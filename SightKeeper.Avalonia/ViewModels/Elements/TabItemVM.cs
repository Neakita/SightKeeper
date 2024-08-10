using Material.Icons;

namespace SightKeeper.Avalonia.ViewModels.Elements;

internal sealed class TabItemViewModel : ViewModel
{
	public MaterialIconKind IconKind { get; }
	public string Header { get; }
	public ViewModel ViewModel { get; }
	
	public TabItemViewModel(MaterialIconKind iconKind, string header, ViewModel viewModel)
	{
		IconKind = iconKind;
		Header = header;
		ViewModel = viewModel;
	}
}