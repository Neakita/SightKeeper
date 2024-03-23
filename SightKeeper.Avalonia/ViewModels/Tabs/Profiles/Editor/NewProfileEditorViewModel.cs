using FluentValidation;
using SightKeeper.Application;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

public sealed class NewProfileEditorViewModel : AbstractProfileEditorVIewModel<NewProfileData>, NewProfileData
{
    public NewProfileEditorViewModel(IValidator<NewProfileData> validator, DataSetsObservableRepository dataSetsObservableRepository) : base(validator, dataSetsObservableRepository, false)
    {
        PreemptionFactorsLink = true;
    }
}