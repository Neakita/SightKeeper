using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Settings.Games;

internal interface BaseGamesSettingsViewModel : SettingsSection
{
	string SettingsSection.Header => "Games";
	IReadOnlyCollection<GameViewModel> Games { get; }
	ICommand AddGameCommand { get; }
}