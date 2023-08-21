using FluentValidation;

namespace SightKeeper.Application.DataSet.Editing;

public sealed class DataSetChangesValidator : AbstractValidator<DataSetChanges>
{
    public DataSetChangesValidator()
    {
        Include(new DataSetInfoValidator());
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