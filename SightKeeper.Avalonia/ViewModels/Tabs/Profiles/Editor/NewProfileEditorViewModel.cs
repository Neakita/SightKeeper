using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

internal sealed class NewProfileEditorViewModel : AbstractProfileEditorViewModel<NewProfileData>, NewProfileData
{
    public NewProfileEditorViewModel(IValidator<NewProfileData> validator, ObservableRepository<DataSet> observableRepository) : base(validator, observableRepository, false)
    {
        PreemptionFactorsLink = true;
    }

    public override string Header => "Create Profile";

    protected override ProfileEditorResult DefaultResult => ProfileEditorResult.Cancel;
}