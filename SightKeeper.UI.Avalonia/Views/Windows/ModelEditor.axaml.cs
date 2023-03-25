using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SightKeeper.UI.Avalonia.Views.Windows;

public partial class ModelEditor : Window
{
	public ModelEditor()
	{
		InitializeComponent();
#if DEBUG
		this.AttachDevTools();
#endif
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}