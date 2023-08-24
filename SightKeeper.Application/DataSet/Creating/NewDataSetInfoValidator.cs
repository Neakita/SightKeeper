using FluentValidation;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application.DataSet.Creating;

public sealed class NewDataSetInfoValidator : AbstractValidator<NewDataSetInfo>
{
    public NewDataSetInfoValidator(IValidator<DataSetInfo> dataSetInfoValidator, DataSetsDataAccess dataSetsDataAccess)
    {
        _dataSetsDataAccess = dataSetsDataAccess;
        Include(dataSetInfoValidator);
        RuleFor(data => data.Name)
            .NotEmpty()
            .MustAsync((dataSet, _, cancellationToken) => NameIsUnique(dataSet, cancellationToken)).WithMessage("Name must be unique");
    }

    private async Task<bool> NameIsUnique(DataSetInfo info, CancellationToken cancellationToken)
    {
        var dataSets = await _dataSetsDataAccess.GetDataSets(cancellationToken);
        return dataSets.All(dataSet => dataSet.Name != info.Name);
    }
    
    private readonly DataSetsDataAccess _dataSetsDataAccess;
}