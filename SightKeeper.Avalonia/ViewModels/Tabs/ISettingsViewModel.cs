using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public interface ISettingsViewModel
{
    IRegisteredGamesViewModel RegisteredGamesViewModel { get; }
}