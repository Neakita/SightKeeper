using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Services;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

public sealed class NewProfileEditorViewModel : AbstractProfileEditorVIewModel<NewProfileData>, NewProfileData
{
    public NewProfileEditorViewModel(IValidator<NewProfileData> validator, DataSetsObservableRepository dataSetsObservableRepository) : base(validator, dataSetsObservableRepository, false)
    {
    }
}