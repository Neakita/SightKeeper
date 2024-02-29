using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles;

public sealed class FakeProfilesViewModel : IProfilesViewModel
{
    public bool MakeScreenshots { get; set; } = true;
    public float MinimumProbability { get; set; } = 0.5f;
    public float MaximumProbability { get; set; } = 0.2f;
    public byte MaximumFPS { get; set; } = 1;
    public IReadOnlyCollection<ProfileViewModel> Profiles { get; }
    public ICommand CreateProfileCommand => FakeViewModel.CommandSubstitute;
    public ICommand EditProfileCommand => FakeViewModel.CommandSubstitute;
    public ICommand LaunchProfileCommand => FakeViewModel.CommandSubstitute;
    public ICommand StopProfileCommand => FakeViewModel.CommandSubstitute;

    public FakeProfilesViewModel()
    {
        DataSet dataSet = new("Dataset 1");
        Game game = new("Game 1", "game1");
        dataSet.Game = game;
        var weights = dataSet.WeightsLibrary.CreateWeights(Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Small,
            100, 0.5f, 0.4f, 0.3f, dataSet.ItemClasses);
        Profile profile1 = new("Profile", string.Empty, 0.5f, 2f, TimeSpan.FromMilliseconds(10), null, weights);
        Profile profile2 = new("Profile with long name!", string.Empty, 0.3f, 3f, TimeSpan.FromMilliseconds(15), null, weights);
        Profiles = new ProfileViewModel[]
        {
            new(profile1),
            new(profile2)
        };
    }
}