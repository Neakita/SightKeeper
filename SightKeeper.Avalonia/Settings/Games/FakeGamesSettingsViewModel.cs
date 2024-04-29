using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Application.Windows;
using SightKeeper.Avalonia.ViewModels;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.Settings.Games;

internal sealed class FakeGamesSettingsViewModel : BaseGamesSettingsViewModel
{
	public IReadOnlyCollection<GameViewModel> Games =>
		[new GameViewModel(new Game("7 Days to Die", "somezombiegame", @"F:\Program Files (x86)\Steam\steamapps\common\7 Days To Die\7DaysToDie.exe"), new WindowsGameIconProvider())];

	public ICommand AddGameCommand => FakeCommand.Instance;
}