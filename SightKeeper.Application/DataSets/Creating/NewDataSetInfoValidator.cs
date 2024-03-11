using FluentValidation;
using SightKeeper.Application.Extensions;

namespace SightKeeper.Application.DataSets.Creating;

public sealed class NewDataSetInfoValidator : AbstractValidator<NewDataSetInfo>
{
    public NewDataSetInfoValidator(IValidator<DataSetInfo> dataSetInfoValidator, DataSetsDataAccess dataSetsDataAccess)
    {
        _dataSetsDataAccess = dataSetsDataAccess;
        Include(dataSetInfoValidator);
        RuleFor(data => data.Resolution)
	        .NotNull()
	        .GreaterThan(0)
	        .LessThanOrEqualTo(ushort.MaxValue)
	        .MultiplierOf(32);
        RuleFor(data => data.Name)
            .NotEmpty()
            .Must((_, dataSetName) => IsNameFree(dataSetName)).WithMessage("Name must be unique");
    }
    
    private readonly DataSetsDataAccess _dataSetsDataAccess;

    private bool IsNameFree(string name)
    {
	    return _dataSetsDataAccess.DataSets.All(dataSet => dataSet.Name != name);
    }
}