using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Settings.Appearance;

[Browsable(true)]
internal sealed partial class AppearanceSettingsViewModel : ViewModel, SettingsSection
{
	public string Header => "Appearance";

	[ObservableProperty]
	[Description("Custom window decorations look like the rest of the window and integrate with the contents, taking up less space. Off by default, using system decorations, providing a native experience.")]
	private bool _customDecorations;

	partial void OnCustomDecorationsChanged(bool value)
	{
		_ = value;
		// ISSUE Should not directly reference window
		ControlExtensions.ReopenWindow<MainWindow>();
	}
}