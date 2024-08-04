using System.ComponentModel;
using SightKeeper.Application;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Settings.Appearance;

[Browsable(true)]
internal sealed class AppearanceSettingsViewModel : ViewModel, SettingsSection
{
	public string Header => "Appearance";

	[Description("Custom window decorations look like the rest of the window and integrate with the contents, taking up less space. Off by default, using system decorations, providing a native experience.")]
	public bool CustomDecorations
	{
		get => _applicationSettingsProvider.CustomDecorations;
		set
		{
			if (value == _applicationSettingsProvider.CustomDecorations)
				return;
			_applicationSettingsProvider.CustomDecorations = value;
			OnPropertyChanged();
			// ISSUE Should not directly reference window
			// ISSUE Probably shouldn't do this directly in the setter
			ControlExtensions.ReopenWindow<MainWindow>();
		}
	}

	public AppearanceSettingsViewModel(ApplicationSettingsProvider applicationSettingsProvider)
	{
		_applicationSettingsProvider = applicationSettingsProvider;
	}

	private readonly ApplicationSettingsProvider _applicationSettingsProvider;
}