using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Services;

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
    public string Description => Profile.Description;
    public Game? Game => _objectsLookupper.GetDataSet(_objectsLookupper.GetLibrary(Profile.Weights)).Game;

    public ProfileViewModel(Profile profile, ObjectsLookupper objectsLookupper)
    {
	    Profile = profile;
	    _objectsLookupper = objectsLookupper;
    }

    public void NotifyChanges()
    {
	    OnPropertiesChanged(Properties);
    }

    private readonly ObjectsLookupper _objectsLookupper;
}