using System.Collections.Generic;
using System.Linq;
using SightKeeper.Avalonia.Settings.Games;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Settings;

internal sealed class FakeSettingsViewModel : ViewModel, ISettingsViewModel
{
	public IReadOnlyCollection<SettingsSection> Sections { get; }
	public SettingsSection SelectedSection { get; set; }

	public FakeSettingsViewModel()
	{
		Sections =
		[
			new FakeGamesSettingsViewModel()
		];
		SelectedSection = Sections.First();
	}
}