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
            .MustAsync((_, dataSetName, cancellationToken) => IsNameFree(dataSetName, cancellationToken)).WithMessage("Name must be unique");
    }
    
    private readonly DataSetsDataAccess _dataSetsDataAccess;

    private Task<bool> IsNameFree(string name, CancellationToken cancellationToken)
    {
        return _dataSetsDataAccess.IsNameFree(name, cancellationToken);
    }
}