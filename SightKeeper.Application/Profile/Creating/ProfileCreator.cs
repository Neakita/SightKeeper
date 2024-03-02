using CommunityToolkit.Diagnostics;
using FluentValidation;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application;

public sealed class ProfileCreator
{
    public ProfileCreator(IValidator<NewProfileData> validator, ProfilesDataAccess profilesDataAccess)
    {
        _validator = validator;
        _profilesDataAccess = profilesDataAccess;
    }
    
    public async Task<Profile> CreateProfile(NewProfileDataDTO data)
    {
        await _validator.ValidateAndThrowAsync(data);
        PreemptionSettings? preemptionSettings;
        if (data.IsPreemptionEnabled)
        {
            Guard.IsNotNull(data.PreemptionHorizontalFactor);
            Guard.IsNotNull(data.PreemptionVerticalFactor);
            if (data.IsPreemptionStabilizationEnabled)
            {
                Guard.IsNotNull(data.PreemptionStabilizationBufferSize);
                Guard.IsNotNull(data.PreemptionStabilizationMethod);
                preemptionSettings = new PreemptionSettings(data.PreemptionHorizontalFactor.Value, data.PreemptionVerticalFactor.Value, data.PreemptionStabilizationBufferSize.Value, data.PreemptionStabilizationMethod.Value);
            }
            else preemptionSettings = new PreemptionSettings(data.PreemptionHorizontalFactor.Value, data.PreemptionVerticalFactor.Value);
        }
        else preemptionSettings = null;
        Profile profile = new(data.Name, data.Description, data.DetectionThreshold, data.MouseSensitivity, data.PostProcessDelay, preemptionSettings, data.Weights);
        foreach (var itemClassData in data.ItemClasses.OrderBy(profileItemClassData => profileItemClassData.Order))
            profile.AddItemClass(itemClassData.ItemClass, itemClassData.ActivationCondition);
        await _profilesDataAccess.AddProfile(profile);
        return profile;
    }
    
    private readonly IValidator<NewProfileData> _validator;
    private readonly ProfilesDataAccess _profilesDataAccess;
}