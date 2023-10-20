using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application;

public sealed class ProfileEditor
{
    public IObservable<Profile> ProfileEdited => _profileEdited.AsObservable();

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
        if (data.IsPreemptionEnabled)
        {
            Guard.IsNotNull(data.PreemptionHorizontalFactor);
            Guard.IsNotNull(data.PreemptionVerticalFactor);
            if (data.IsPreemptionStabilizationEnabled)
            {
                Guard.IsNotNull(data.PreemptionStabilizationBufferSize);
                Guard.IsNotNull(data.PreemptionStabilizationMethod);
                profile.PreemptionSettings = new PreemptionSettings(data.PreemptionHorizontalFactor.Value, data.PreemptionVerticalFactor.Value, data.PreemptionStabilizationBufferSize.Value, data.PreemptionStabilizationMethod.Value);
            }
            else profile.PreemptionSettings = new PreemptionSettings(data.PreemptionHorizontalFactor.Value, data.PreemptionVerticalFactor.Value);
        }
        else profile.PreemptionSettings = null;

        profile.PostProcessDelay = data.PostProcessDelay;
        profile.Weights = data.Weights;
        profile.ClearItemClasses();
        foreach (var itemClassData in data.ItemClasses)
            profile.AddItemClass(itemClassData.ItemClass, itemClassData.ActivationCondition);
        await _profilesDataAccess.UpdateProfile(profile);
        _profileEdited.OnNext(profile);
    }

    private readonly IValidator<EditedProfileData> _validator;
    private readonly ProfilesDataAccess _profilesDataAccess;
    private readonly Subject<Profile> _profileEdited = new();
}