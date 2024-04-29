using System.Collections.Generic;

namespace SightKeeper.Avalonia.Settings;

internal interface ISettingsViewModel
{
	IReadOnlyCollection<SettingsSection> Sections { get; }
	SettingsSection SelectedSection { get; set; }
}