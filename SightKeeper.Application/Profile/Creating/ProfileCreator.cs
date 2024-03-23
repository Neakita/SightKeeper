using CommunityToolkit.Diagnostics;
using FluentValidation;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Application;

public sealed class ProfileCreator
{
    public ProfileCreator(IValidator<NewProfileData> validator, ProfilesDataAccess profilesDataAccess)
    {
        _validator = validator;
        _profilesDataAccess = profilesDataAccess;
    }
    
    public Profile CreateProfile(NewProfileDataDTO data)
    {
        _validator.ValidateAndThrow(data);
        PreemptionSettings? preemptionSettings;
        if (data.IsPreemptionEnabled)
        {
            Guard.IsNotNull(data.PreemptionHorizontalFactor);
            Guard.IsNotNull(data.PreemptionVerticalFactor);
            Vector2<float> preemptionFactors =
	            new(data.PreemptionHorizontalFactor.Value, data.PreemptionVerticalFactor.Value);
            if (data.IsPreemptionStabilizationEnabled)
            {
                Guard.IsNotNull(data.PreemptionStabilizationBufferSize);
                Guard.IsNotNull(data.PreemptionStabilizationMethod);
                StabilizationSettings stabilizationSettings = new(data.PreemptionStabilizationBufferSize.Value,
	                data.PreemptionStabilizationMethod.Value);
                preemptionSettings = new PreemptionSettings(preemptionFactors, stabilizationSettings);
            }
            else preemptionSettings = new PreemptionSettings(preemptionFactors);
        }
        else preemptionSettings = null;
        Profile profile = new(data.Name, data.Description, data.DetectionThreshold, data.MouseSensitivity, data.PostProcessDelay, preemptionSettings, data.Weights);
        foreach (var itemClassData in data.ItemClasses.OrderBy(profileItemClassData => profileItemClassData.Order))
            profile.AddItemClass(itemClassData.ItemClass, itemClassData.ActivationCondition);
        _profilesDataAccess.AddProfile(profile);
        return profile;
    }
    
    private readonly IValidator<NewProfileData> _validator;
    private readonly ProfilesDataAccess _profilesDataAccess;
}