using FluentValidation;
using SightKeeper.Commons.Validation;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application.DataSet;

public sealed class DataSetInfoValidator : AbstractValidator<DataSetInfo>
{
    public DataSetInfoValidator(DataSetsDataAccess dataSetsDataAccess)
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
    }

    private async Task<bool> NameIsUnique(DataSetInfo info, CancellationToken cancellationToken)
    {
        var dataSets = await _dataSetsDataAccess.GetDataSets(cancellationToken);
        return dataSets.All(dataSet => dataSet.Name != info.Name);
    }
    
    private readonly DataSetsDataAccess _dataSetsDataAccess;
}