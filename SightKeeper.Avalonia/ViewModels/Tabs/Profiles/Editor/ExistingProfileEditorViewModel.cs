using System;
using System.Linq;
using DynamicData;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

internal sealed class ExistingProfileEditorViewModel : AbstractProfileEditorViewModel<EditedProfileData>, EditedProfileData
{
    public ExistingProfileEditorViewModel(IValidator<EditedProfileData> validator, DataSetsObservableRepository dataSetsObservableRepository) : base(validator, dataSetsObservableRepository, true)
    {
    }
    
    public void SetData(Profile profile)
    {
        using var disposable = Validator.SuppressValidation();
        Profile = profile;
        Name = profile.Name;
        Description = profile.Description;
        DetectionThreshold = profile.DetectionThreshold;
        MouseSensitivity = profile.MouseSensitivity;
        PostProcessDelay = profile.PostProcessDelay;
        DataSet = profile.Weights.Library.DataSet;
        Weights = profile.Weights;
        ItemClassesSource.Clear();
        ItemClassesSource.AddRange(profile.ItemClasses
            .Select(profileItemClass => new ProfileItemClassViewModel(profileItemClass.ItemClass, (byte)profile.ItemClasses.IndexOf(profileItemClass), profileItemClass.ActivationCondition)));
        if (profile.PreemptionSettings != null)
        {
            IsPreemptionEnabled = true;
            PreemptionHorizontalFactor = profile.PreemptionSettings.Factor.X;
            PreemptionVerticalFactor = profile.PreemptionSettings.Factor.Y;
            if (profile.PreemptionSettings.StabilizationSettings != null)
            {
                IsPreemptionStabilizationEnabled = true;
                PreemptionStabilizationBufferSize = profile.PreemptionSettings.StabilizationSettings.BufferSize;
                PreemptionStabilizationMethod = profile.PreemptionSettings.StabilizationSettings.Method;
            }

            var factorsDifference = Math.Abs(PreemptionHorizontalFactor.Value) - Math.Abs(PreemptionVerticalFactor.Value);
            const float factorsDifferenceTolerance = 0.01f;
            PreemptionFactorsLink = factorsDifference < factorsDifferenceTolerance;
        }
    }

    public override string Header => "Edit profile";
    protected override ProfileEditorResult DefaultResult => ProfileEditorResult.Cancel;
}