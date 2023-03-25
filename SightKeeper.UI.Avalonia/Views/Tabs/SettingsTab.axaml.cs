using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SightKeeper.UI.Avalonia.ViewModels.Tabs;

namespace SightKeeper.UI.Avalonia.Views.Tabs;

public partial class SettingsTab : ReactiveUserControl<SettingsTabViewModel>
{
	public SettingsTab()
	{
		
	}
	
	public SettingsTab(SettingsTabViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}