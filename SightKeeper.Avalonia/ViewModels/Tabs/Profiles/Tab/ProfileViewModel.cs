using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Tab;

internal sealed class ProfileViewModel : ViewModel
{

	private static readonly string[] Properties =
    {
        "Name",
        "Description",
        "Game"
    };
    
    public Profile Profile { get; }
    public string Name => Profile.Name;
    // public string Description => Profile.Description;
    // public Game? Game => _objectsLookupper.GetDataSet(_objectsLookupper.GetLibrary(Profile.Weights)).Game;

    public ProfileViewModel(Profile profile)
    {
	    Profile = profile;
    }

    public void NotifyChanges()
    {
	    OnPropertiesChanged(Properties);
    }
}