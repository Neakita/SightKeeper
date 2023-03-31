using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SightKeeper.UI.Avalonia.ViewModels.Tabs;

namespace SightKeeper.UI.Avalonia.Views.Tabs;

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