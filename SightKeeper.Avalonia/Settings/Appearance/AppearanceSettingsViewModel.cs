using System.ComponentModel;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Settings.Appearance;

[Browsable(true)]
internal sealed class AppearanceSettingsViewModel : ViewModel, SettingsSection
{
	public string Header => "Appearance";

	[Description("Custom window decorations look like the rest of the window and integrate with the contents, taking up less space. Off by default, using system decorations, providing a native experience.")]
	public bool CustomDecorations
	{
		get => _customDecorations;
		set
		{
			if (value == _customDecorations)
				return;
			_customDecorations = value;
			OnPropertyChanged();
			
		}
	}

	private bool _customDecorations;
}