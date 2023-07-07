using FluentValidation;
using SightKeeper.Commons.Validation;

namespace SightKeeper.Application.Modelling;

public sealed class ModelChangesValidator : AbstractValidator<ModelChanges>
{
    public ModelChangesValidator()
    {
        RuleFor(changes => changes.ResolutionWidth)
            .GreaterThan(0)
            .LessThanOrEqualTo(3200)
            .MultiplierOf(32)

            .Must((changes, resolutionWidth) => resolutionWidth == changes.Model.Resolution.Width)
            .Unless(changes => changes.Model.CanChangeResolution(out _), ApplyConditionTo.CurrentValidator)
            .WithMessage(changes =>
            {
                changes.Model.CanChangeResolution(out var message);
                return $"Resolution can't be changed: {message}";
            });
            
        RuleFor(changes => changes.ResolutionHeight)
            .GreaterThan(0)
            .LessThanOrEqualTo(3200)
            .MultiplierOf(32)
            
            .Must((changes, resolutionWidth) => resolutionWidth == changes.Model.Resolution.Width)
            .Unless(changes => changes.Model.CanChangeResolution(out _), ApplyConditionTo.CurrentValidator)
            .WithMessage(changes =>
            {
                changes.Model.CanChangeResolution(out var message);
                return $"Resolution can't be changed: {message}";
            });;
        
        RuleFor(changes => changes.ItemClasses).NoDuplicates();
    }
}