using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public interface IRegisteredGamesViewModel
{
    IReadOnlyCollection<Game> RegisteredGames { get; }
    IReadOnlyCollection<Game> AvailableToAddGames { get; }
    
    Game? SelectedToAddGame { get; set; }
    Game? SelectedExistingGame { get; set; }
    
    ICommand AddGameCommand { get; }
    ICommand DeleteGameCommand { get; }
    ICommand RefreshAvailableToAddGamesCommand { get; }
}