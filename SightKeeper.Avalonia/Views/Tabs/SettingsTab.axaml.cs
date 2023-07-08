using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Tabs;

namespace SightKeeper.Avalonia.Views.Tabs;

public sealed partial class SettingsTab : ReactiveUserControl<SettingsTabViewModel>
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