using FluentValidation;
using SightKeeper.Commons.Validation;

namespace SightKeeper.Application.DataSet;

public sealed class DataSetInfoValidator : AbstractValidator<DataSetInfo>
{
    public DataSetInfoValidator()
    {
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
}