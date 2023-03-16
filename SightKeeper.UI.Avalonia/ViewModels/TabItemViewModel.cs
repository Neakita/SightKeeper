using Avalonia.Controls;

namespace SightKeeper.UI.Avalonia.ViewModels;

public sealed class TabItemViewModel : ViewModelBase
{
	public string Header { get; }
	public Control Content { get; }
	
	public TabItemViewModel(string header, Control content)
	{
		Header = header;
		Content = content;
	}
}
