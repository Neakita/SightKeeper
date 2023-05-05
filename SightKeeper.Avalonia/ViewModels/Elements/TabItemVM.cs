using Avalonia.Controls;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class TabItem : ViewModel
{
	public string Header { get; }
	public Control Content { get; }
	
	public TabItem(string header, Control content)
	{
		Header = header;
		Content = content;
	}
}
