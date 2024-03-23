﻿using System.Linq;
using DynamicData;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Services;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

public sealed class ExistingProfileEditorViewModel : AbstractProfileEditorVIewModel<EditedProfileData>, EditedProfileData
{
    public ExistingProfileEditorViewModel(IValidator<EditedProfileData> validator, DataSetsObservableRepository dataSetsObservableRepository) : base(validator, dataSetsObservableRepository, true)
    {
    }
    
    public void SetData(Profile profile)
    {
        ValidateOnPropertyChanged = false;
        Profile = profile;
        Name = profile.Name;
        Description = profile.Description;
        DetectionThreshold = profile.DetectionThreshold;
        MouseSensitivity = profile.MouseSensitivity;
        PostProcessDelay = profile.PostProcessDelay;
        DataSet = profile.Weights.Library.DataSet;
        Weights = profile.Weights;
        _itemClasses.Clear();
        _itemClasses.AddRange(profile.ItemClasses
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
            PreemptionFactorsLink = PreemptionHorizontalFactor == PreemptionVerticalFactor;
        }
        ValidateOnPropertyChanged = true;
    }
}