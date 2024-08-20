using FluentValidation;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.DataSets.Creating;

public sealed class NewDataSetInfoValidator : AbstractValidator<NewDataSetInfo>
{
    public NewDataSetInfoValidator(IValidator<GeneralDataSetInfo> dataSetInfoValidator, ReadDataAccess<DataSet> dataSetsDataAccess)
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
    
    private readonly ReadDataAccess<DataSet> _dataSetsDataAccess;

    private bool IsNameFree(string name)
    {
	    return _dataSetsDataAccess.Items.All(dataSet => dataSet.Name != name);
    }
}