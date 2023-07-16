using FluentValidation;
using SightKeeper.Commons.Validation;

namespace SightKeeper.Application.Model;

public sealed class ModelDataValidator : AbstractValidator<ModelData>
{
    public ModelDataValidator()
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