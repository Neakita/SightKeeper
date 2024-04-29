using Avalonia.ReactiveUI;

namespace SightKeeper.Avalonia.Settings;

internal sealed partial class SettingsTab : ReactiveUserControl<SettingsViewModel>
{
	public SettingsTab()
	{
		InitializeComponent();
	}
}