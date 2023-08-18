using FluentValidation;
using SightKeeper.Commons.Validation;

namespace SightKeeper.Application.Model;

public sealed class DataSetDataValidator : AbstractValidator<DataSetData>
{
    public DataSetDataValidator()
    {
        RuleFor(data => data.ResolutionWidth)
            .NotNull()
            .GreaterThan(0)
            .LessThanOrEqualTo(3200)
            .MultiplierOf(32);

        RuleFor(changes => changes.ResolutionHeight)
            .NotNull()
            .GreaterThan(0)
            .LessThanOrEqualTo(3200)
            .MultiplierOf(32);
        
        RuleFor(changes => changes.ItemClasses).NoDuplicates();
    }
}