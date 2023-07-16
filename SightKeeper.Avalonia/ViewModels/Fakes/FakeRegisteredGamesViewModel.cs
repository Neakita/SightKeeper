using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeRegisteredGamesViewModel : IRegisteredGamesViewModel
{
    public Task<IReadOnlyCollection<Game>> RegisteredGames { get; }
    public Task<IReadOnlyCollection<Game>> AvailableToAddGames { get; }
    public Game? SelectedToAddGame { get; set; }
    public Game? SelectedExistingGame { get; set; }
    public IAsyncRelayCommand AddGameCommand { get; }
    public IAsyncRelayCommand DeleteGameCommand { get; }
    public IRelayCommand RefreshAvailableToAddGamesCommand { get; }
}