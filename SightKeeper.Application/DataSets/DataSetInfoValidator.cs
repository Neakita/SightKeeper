using FluentValidation;
using SightKeeper.Commons.Validation;

namespace SightKeeper.Application.DataSets;

public sealed class DataSetInfoValidator : AbstractValidator<DataSetInfo>
{
    public DataSetInfoValidator()
    {
        RuleFor(data => data.Resolution)
            .NotNull()
            .GreaterThan(0)
            .LessThanOrEqualTo(ushort.MaxValue)
            .MultiplierOf(32);
        
        RuleFor(changes => changes.ItemClasses).NoDuplicates(itemClass => itemClass.Name);
    }
}