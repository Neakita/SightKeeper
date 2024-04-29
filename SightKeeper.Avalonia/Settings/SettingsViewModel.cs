using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Settings;

internal sealed partial class SettingsViewModel : ViewModel, ISettingsViewModel
{
	public IReadOnlyCollection<SettingsSection> Sections { get; }

	public SettingsViewModel(IEnumerable<SettingsSection> sections)
	{
		Sections = sections.ToImmutableList();
		_selectedSection = Sections.First();
	}

	[ObservableProperty]
	private SettingsSection _selectedSection;
}