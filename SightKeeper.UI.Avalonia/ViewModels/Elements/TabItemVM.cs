using Avalonia.Controls;

namespace SightKeeper.UI.Avalonia.ViewModels.Elements;

public sealed class TabItemViewModel : ViewModel
{
	public string Header { get; }
	public Control Content { get; }
	
	public TabItemViewModel(string header, Control content)
	{
		Header = header;
		Content = content;
	}
}
