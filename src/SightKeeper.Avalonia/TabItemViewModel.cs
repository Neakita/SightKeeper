using Material.Icons;

namespace SightKeeper.Avalonia;

public sealed class TabItemViewModel : ViewModel
{
	public MaterialIconKind IconKind { get; }
	public string Header { get; }
	public ViewModel Content { get; }
	
	public TabItemViewModel(MaterialIconKind iconKind, string header, ViewModel content)
	{
		IconKind = iconKind;
		Header = header;
		Content = content;
	}
}