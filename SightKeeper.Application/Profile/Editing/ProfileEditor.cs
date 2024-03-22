using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using SightKeeper.Domain;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Application;

public class ProfileEditor
{
    public IObservable<Profile> ProfileEdited => _profileEdited.AsObservable();

    public ProfileEditor(IValidator<EditedProfileData> validator)
    {
        _validator = validator;
    }

    public virtual void ApplyChanges(EditedProfileDataDTO data)
    {
        _validator.ValidateAndThrow(data);
        var profile = data.Profile;
        profile.Name = data.Name;
        profile.Description = data.Description;
        profile.DetectionThreshold = data.DetectionThreshold;
        profile.MouseSensitivity = data.MouseSensitivity;
        if (data.IsPreemptionEnabled)
        {
            Guard.IsNotNull(data.PreemptionHorizontalFactor);
            Guard.IsNotNull(data.PreemptionVerticalFactor);
            Vector2<float> preemptionFactors = new(data.PreemptionHorizontalFactor.Value, data.PreemptionVerticalFactor.Value);
            if (data.IsPreemptionStabilizationEnabled)
            {
                Guard.IsNotNull(data.PreemptionStabilizationBufferSize);
                Guard.IsNotNull(data.PreemptionStabilizationMethod);
                StabilizationSettings stabilizationSettings = new(data.PreemptionStabilizationBufferSize.Value,
	                data.PreemptionStabilizationMethod.Value);
                profile.PreemptionSettings = new PreemptionSettings(preemptionFactors, stabilizationSettings);
            }
            else profile.PreemptionSettings = new PreemptionSettings(preemptionFactors);
        }
        else profile.PreemptionSettings = null;

        profile.PostProcessDelay = data.PostProcessDelay;
        profile.Weights = data.Weights;
        profile.ClearItemClasses();
        foreach (var itemClassData in data.ItemClasses)
            profile.AddItemClass(itemClassData.ItemClass, itemClassData.ActivationCondition);
        _profileEdited.OnNext(profile);
    }

    private readonly IValidator<EditedProfileData> _validator;
    private readonly Subject<Profile> _profileEdited = new();
}