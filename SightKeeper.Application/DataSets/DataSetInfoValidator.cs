using FluentValidation;

namespace SightKeeper.Application.DataSets;

public sealed class DataSetInfoValidator : AbstractValidator<GeneralDataSetInfo>
{
    public DataSetInfoValidator()
    {
    }
}