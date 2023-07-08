using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Tabs;

namespace SightKeeper.Avalonia.Views.Tabs;

public sealed partial class SettingsTab : ReactiveUserControl<SettingsViewModel>
{
	public SettingsTab()
	{
		
	}
	
	public SettingsTab(SettingsViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}