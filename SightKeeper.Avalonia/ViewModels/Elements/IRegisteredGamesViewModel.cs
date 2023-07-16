using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public interface IRegisteredGamesViewModel
{
    Task<IReadOnlyCollection<Game>> RegisteredGames { get; }
    Task<IReadOnlyCollection<Game>> AvailableToAddGames { get; }
    
    Game? SelectedToAddGame { get; set; }
    Game? SelectedExistingGame { get; set; }
    
    IAsyncRelayCommand AddGameCommand { get; }
    IAsyncRelayCommand DeleteGameCommand { get; }
    IRelayCommand RefreshAvailableToAddGamesCommand { get; }
}