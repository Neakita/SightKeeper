using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public interface IRegisteredGamesViewModel
{
    Task<IReadOnlyCollection<Game>> RegisteredGames { get; }
    Task<IReadOnlyCollection<Game>> AvailableToAddGames { get; }
    
    Game? SelectedToAddGame { get; set; }
    Game? SelectedExistingGame { get; set; }
    
    ICommand AddGameCommand { get; }
    ICommand DeleteGameCommand { get; }
    ICommand RefreshAvailableToAddGamesCommand { get; }
}