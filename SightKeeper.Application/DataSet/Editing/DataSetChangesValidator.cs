using FluentValidation;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application.DataSet.Editing;

public sealed class DataSetChangesValidator : AbstractValidator<DataSetChanges>
{
    public DataSetChangesValidator(IValidator<DataSetInfo> dataSetInfoValidator, DataSetsDataAccess dataSetsDataAccess)
    {
        _dataSetsDataAccess = dataSetsDataAccess;
        
        Include(dataSetInfoValidator);
        
        RuleFor(data => data.Name)
            .NotEmpty()
            .MustAsync((dataSet, _, cancellationToken) => NameIsUnique(dataSet, cancellationToken)).WithMessage("Name must be unique");
        
        RuleFor(changes => changes.Resolution)
            .Must((changes, resolution) => resolution == changes.DataSet.Resolution)
            .Unless(changes => changes.DataSet.CanChangeResolution(out _), ApplyConditionTo.CurrentValidator)
            .WithMessage(changes =>
            {
                changes.DataSet.CanChangeResolution(out var message);
                return $"Resolution can't be changed: {message}";
            });
    }

    private async Task<bool> NameIsUnique(DataSetChanges changes, CancellationToken cancellationToken)
    {
        var dataSets = await _dataSetsDataAccess.GetDataSets(cancellationToken);
        dataSets = dataSets.Where(dataSet => dataSet != changes.DataSet).ToList();
        return dataSets.All(dataSet => dataSet.Name != changes.Name);
    }
    
    private readonly DataSetsDataAccess _dataSetsDataAccess;
}