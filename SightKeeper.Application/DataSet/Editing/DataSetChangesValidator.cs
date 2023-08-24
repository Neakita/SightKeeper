using FluentValidation;
using SightKeeper.Commons.Validation;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application.DataSet.Editing;

public sealed class DataSetChangesValidator : AbstractValidator<DataSetChanges>
{
    public DataSetChangesValidator(DataSetsDataAccess dataSetsDataAccess)
    {
        _dataSetsDataAccess = dataSetsDataAccess;
        
        RuleFor(data => data.Name)
            .NotEmpty()
            .MustAsync((dataSet, _, cancellationToken) => NameIsUnique(dataSet, cancellationToken)).WithMessage("Name must be unique");
        
        RuleFor(data => data.ResolutionWidth)
            .NotNull()
            .GreaterThan(0)
            .LessThanOrEqualTo(ushort.MaxValue)
            .MultiplierOf(32);

        RuleFor(changes => changes.ResolutionHeight)
            .NotNull()
            .GreaterThan(0)
            .LessThanOrEqualTo(ushort.MaxValue)
            .MultiplierOf(32);
        
        RuleFor(changes => changes.ItemClasses).NoDuplicates();
        
        RuleFor(changes => changes.ResolutionWidth)
            .Must((changes, resolutionWidth) => resolutionWidth == changes.DataSet.Resolution.Width)
            .Unless(changes => changes.DataSet.CanChangeResolution(out _), ApplyConditionTo.CurrentValidator)
            .WithMessage(changes =>
            {
                changes.DataSet.CanChangeResolution(out var message);
                return $"Resolution can't be changed: {message}";
            });
            
        RuleFor(changes => changes.ResolutionHeight)
            .Must((changes, resolutionWidth) => resolutionWidth == changes.DataSet.Resolution.Width)
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