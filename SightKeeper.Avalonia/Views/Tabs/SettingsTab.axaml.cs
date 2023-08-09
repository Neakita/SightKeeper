using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Tabs;

namespace SightKeeper.Avalonia.Views.Tabs;

public sealed partial class SettingsTab : ReactiveUserControl<SettingsViewModel>
{
	public SettingsTab()
	{
		InitializeComponent();
	}
}