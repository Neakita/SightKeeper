using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using NSubstitute;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeRegisteredGamesViewModel : IRegisteredGamesViewModel
{
    public Task<IReadOnlyCollection<Game>> RegisteredGames { get; } = Task.FromResult((IReadOnlyCollection<Game>)new[]
    {
        new Game("Test Game 1", "process 1"),
        new Game("Test Game 2", "process 2"),
    });
    public Task<IReadOnlyCollection<Game>> AvailableToAddGames { get; } = Task.FromResult((IReadOnlyCollection<Game>)new[]
    {
        new Game("Test Game 3", "process 3"),
        new Game("Test Game 4", "process 4"),
    });
    public Game? SelectedToAddGame { get; set; }
    public Game? SelectedExistingGame { get; set; }
    public ICommand AddGameCommand { get; } = Substitute.For<ICommand>();
    public ICommand DeleteGameCommand { get; } = Substitute.For<ICommand>();
    public ICommand RefreshAvailableToAddGamesCommand { get; } = Substitute.For<ICommand>();
}