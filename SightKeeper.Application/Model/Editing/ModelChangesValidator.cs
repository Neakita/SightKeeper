using FluentValidation;

namespace SightKeeper.Application.Model.Editing;

public sealed class ModelChangesValidator : AbstractValidator<DataSetDataChanges>
{
    public ModelChangesValidator()
    {
        Include(new DataSetDataValidator());
        RuleFor(changes => changes.ResolutionWidth)
            .Must((changes, resolutionWidth) => resolutionWidth == changes.DataSet.Resolution.Width)
            .Unless(changes => changes.DataSet.CanChangeResolution(out _), ApplyConditionTo.CurrentValidator)
            .WithMessage(changes =>
            {
                changes.DataSet.CanChangeResolution(out var message);
                return $"Resolution can't be changed: {message}";
            });
            
        RuleFor(changes => changes.ResolutionHeight)
            .Must((changes, resolutionWidth) => resolutionWidth == changes.DataSet.Resolution.Width)
            .Unless(changes => changes.DataSet.CanChangeResolution(out _), ApplyConditionTo.CurrentValidator)
            .WithMessage(changes =>
            {
                changes.DataSet.CanChangeResolution(out var message);
                return $"Resolution can't be changed: {message}";
            });
    }
}