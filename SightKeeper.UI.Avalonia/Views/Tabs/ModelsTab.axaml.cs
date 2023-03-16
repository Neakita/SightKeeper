using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SightKeeper.UI.Avalonia.Views.Tabs;

public partial class ModelsTab : UserControl
{
	public ModelsTab()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}