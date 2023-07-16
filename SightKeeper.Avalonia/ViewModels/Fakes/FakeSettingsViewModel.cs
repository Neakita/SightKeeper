using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Avalonia.ViewModels.Tabs;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeSettingsViewModel : ISettingsViewModel
{
    public IRegisteredGamesViewModel? RegisteredGamesViewModel => new FakeRegisteredGamesViewModel();
    public IConfigsViewModel? ConfigsViewModel => new FakeConfigsViewModel();
}