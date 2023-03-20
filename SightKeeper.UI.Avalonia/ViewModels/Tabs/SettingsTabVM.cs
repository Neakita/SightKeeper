using SightKeeper.Infrastructure.Data;
using SightKeeper.Infrastructure.Services;
using SightKeeper.UI.Avalonia.ViewModels.Elements;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class SettingsTabVM
{
	public RegisteredGamesVM RegisteredGamesVM { get; }

	public SettingsTabVM()
	{
		RegisteredGamesVM = new RegisteredGamesVM(new DbGamesRegistrator(new DefaultAppDbContextFactory()));
	}
	
	public SettingsTabVM(RegisteredGamesVM registeredGamesVM)
	{
		RegisteredGamesVM = registeredGamesVM;
	}
}
