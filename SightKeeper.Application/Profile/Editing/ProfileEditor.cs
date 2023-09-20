using System.Reactive.Linq;
using FluentValidation;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application;

public sealed class ProfileEditor
{
    public IObservable<Profile> ProfileEdited { get; } = Observable.Empty<Profile>();

    public ProfileEditor(IValidator<EditedProfileData> validator, ProfilesDataAccess profilesDataAccess)
    {
        _validator = validator;
        _profilesDataAccess = profilesDataAccess;
    }

    public async Task ApplyChanges(EditedProfileDataDTO data)
    {
        await _validator.ValidateAndThrowAsync(data);
        var profile = data.Profile;
        profile.Name = data.Name;
        profile.Description = data.Description;
        profile.DetectionThreshold = data.DetectionThreshold;
        profile.MouseSensitivity = data.MouseSensitivity;
        profile.Weights = data.Weights;
        profile.ClearItemClasses();
        foreach (var itemClass in data.ItemClasses)
            profile.AddItemClass(itemClass);
        await _profilesDataAccess.UpdateProfile(profile);
    }

    private readonly IValidator<EditedProfileData> _validator;
    private readonly ProfilesDataAccess _profilesDataAccess;
}