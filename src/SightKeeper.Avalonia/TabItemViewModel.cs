using Material.Icons;

namespace SightKeeper.Avalonia;

public sealed class TabItemViewModel(MaterialIconKind iconKind, string header, object? content) : ViewModel
{
	public MaterialIconKind IconKind => iconKind;
	public string Header => header;
	public object? Content => content;
}