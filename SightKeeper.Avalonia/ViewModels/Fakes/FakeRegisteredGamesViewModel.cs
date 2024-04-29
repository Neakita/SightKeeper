using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeRegisteredGamesViewModel : IRegisteredGamesViewModel
{
    public IReadOnlyCollection<Game> RegisteredGames { get; } = new[]
    {
	    new Game("Test Game 1", "process 1"),
	    new Game("Test Game 2", "process 2"),
    };
    public IReadOnlyCollection<Game> AvailableToAddGames { get; } = new[]
    {
	    new Game("Test Game 3", "process 3"),
	    new Game("Test Game 4", "process 4"),
    };
    public Game? SelectedToAddGame { get; set; }
    public Game? SelectedExistingGame { get; set; }
    public ICommand AddGameCommand { get; } = FakeCommand.Instance;
    public ICommand DeleteGameCommand { get; } = FakeCommand.Instance;
    public ICommand RefreshAvailableToAddGamesCommand { get; } = FakeCommand.Instance;
}