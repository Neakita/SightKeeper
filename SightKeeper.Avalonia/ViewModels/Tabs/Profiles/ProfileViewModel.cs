using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles;

public sealed class ProfileViewModel
{
    public Profile Profile { get; }
    public string Name => Profile.Name;
    public string Description => Profile.Description;
    public Game? Game => Profile.Weights.Library.DataSet.Game;

    public ProfileViewModel(Profile profile)
    {
        Profile = profile;
    }
}