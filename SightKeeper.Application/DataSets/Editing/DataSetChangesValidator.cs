using FluentValidation;

namespace SightKeeper.Application.DataSets.Editing;

public sealed class DataSetChangesValidator : AbstractValidator<DataSetChanges>
{
    public DataSetChangesValidator(IValidator<DataSetInfo> dataSetInfoValidator, DataSetsDataAccess dataSetsDataAccess)
    {
        _dataSetsDataAccess = dataSetsDataAccess;
        
        Include(dataSetInfoValidator);
        
        RuleFor(data => data.Name)
            .NotEmpty()
            .Must((dataSet, _) => IsNameFree(dataSet)).WithMessage("Name must be unique");
    }

    private bool IsNameFree(DataSetChanges changes)
    {
	    return _dataSetsDataAccess.DataSets.All(dataSet => dataSet == changes.DataSet || dataSet.Name != changes.Name);
    }

    private readonly DataSetsDataAccess _dataSetsDataAccess;
}