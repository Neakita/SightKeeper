using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SightKeeper.UI.Avalonia.ViewModels.Tabs;

namespace SightKeeper.UI.Avalonia.Views.Tabs;

public partial class SettingsTab : UserControl
{
	public SettingsTab()
	{
		
	}
	
	public SettingsTab(SettingsTabVM viewModel)
	{
		InitializeComponent();
		DataContext = viewModel;
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}