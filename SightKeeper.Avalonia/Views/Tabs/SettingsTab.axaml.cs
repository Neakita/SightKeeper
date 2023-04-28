using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Tabs;

namespace SightKeeper.Avalonia.Views.Tabs;

public partial class SettingsTab : ReactiveUserControl<SettingsTabVM>
{
	public SettingsTab()
	{
		
	}
	
	public SettingsTab(SettingsTabVM vm)
	{
		InitializeComponent();
		ViewModel = vm;
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}