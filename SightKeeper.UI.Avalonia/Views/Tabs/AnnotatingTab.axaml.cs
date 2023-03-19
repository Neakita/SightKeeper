using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SightKeeper.UI.Avalonia.Views.Tabs;

public partial class AnnotatingTab : UserControl
{
	public AnnotatingTab()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}