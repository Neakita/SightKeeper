using FluentValidation;

namespace SightKeeper.Application.Model.Editing;

public sealed class ModelChangesValidator : AbstractValidator<ModelChanges>
{
    public ModelChangesValidator()
    {
        Include(new ModelDataValidator());
        RuleFor(changes => changes.ResolutionWidth)
            .Must((changes, resolutionWidth) => resolutionWidth == changes.Model.Resolution.Width)
            .Unless(changes => changes.Model.CanChangeResolution(out _), ApplyConditionTo.CurrentValidator)
            .WithMessage(changes =>
            {
                changes.Model.CanChangeResolution(out var message);
                return $"Resolution can't be changed: {message}";
            });
            
        RuleFor(changes => changes.ResolutionHeight)
            .Must((changes, resolutionWidth) => resolutionWidth == changes.Model.Resolution.Width)
            .Unless(changes => changes.Model.CanChangeResolution(out _), ApplyConditionTo.CurrentValidator)
            .WithMessage(changes =>
            {
                changes.Model.CanChangeResolution(out var message);
                return $"Resolution can't be changed: {message}";
            });
    }
}