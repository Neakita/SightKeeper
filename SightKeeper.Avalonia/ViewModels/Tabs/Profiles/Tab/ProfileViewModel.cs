using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles;

public sealed class ProfileViewModel : ViewModel
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
    public Game? Game => Profile.Weights.Library.DataSet.Game;

    public ProfileViewModel(Profile profile)
    {
        Profile = profile;
    }

    public void NotifyChanges()
    {
        OnPropertiesChanged(Properties);
    }
}