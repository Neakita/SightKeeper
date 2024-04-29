using FluentValidation;
using SightKeeper.Application;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

internal sealed class NewProfileEditorViewModel : AbstractProfileEditorViewModel<NewProfileData>, NewProfileData
{
    public NewProfileEditorViewModel(IValidator<NewProfileData> validator, DataSetsObservableRepository dataSetsObservableRepository) : base(validator, dataSetsObservableRepository, false)
    {
        PreemptionFactorsLink = true;
    }

    public override string Header => "Create Profile";

    protected override ProfileEditorResult DefaultResult => ProfileEditorResult.Cancel;
}