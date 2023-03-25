using SightKeeper.Infrastructure.Data;
using SightKeeper.Infrastructure.Services;
using SightKeeper.UI.Avalonia.ViewModels.Elements;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class SettingsTabViewModel
{
	public RegisteredGamesVM RegisteredGamesVM { get; }

	public SettingsTabViewModel()
	{
		RegisteredGamesVM = new RegisteredGamesVM(new DbGamesRegistrator(new DefaultAppDbContextFactory()));
	}
	
	public SettingsTabViewModel(RegisteredGamesVM registeredGamesVM)
	{
		RegisteredGamesVM = registeredGamesVM;
	}
}
