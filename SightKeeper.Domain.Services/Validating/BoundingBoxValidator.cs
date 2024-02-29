using FluentValidation;
using SightKeeper.Domain.Model.DataSets.Screenshots.Assets.Detector;

namespace SightKeeper.Domain.Services.Validating;

public sealed class BoundingBoxValidator : AbstractValidator<Bounding>
{
    public BoundingBoxValidator()
    {
        RuleFor(boundingBox => boundingBox.Left)
            .Must(x => x >= 0).WithMessage("{PropertyName} must be greater than or equal to 0")
            .Must(x => x <= 1).WithMessage("{PropertyName} must be less than or equal to 1");
        RuleFor(boundingBox => boundingBox.Top)
            .Must(y => y >= 0).WithMessage("{PropertyName} must be greater than or equal to 0")
            .Must(y => y <= 1).WithMessage("{PropertyName} must be less than or equal to 1");
        RuleFor(boundingBox => boundingBox.Right)
            .Must(x => x >= 0).WithMessage("{PropertyName} must be greater than or equal to 0")
            .Must(x => x <= 1).WithMessage("{PropertyName} must be less than or equal to 1");
        RuleFor(boundingBox => boundingBox.Bottom)
            .Must(y => y >= 0).WithMessage("{PropertyName} must be greater than or equal to 0")
            .Must(y => y <= 1).WithMessage("{PropertyName} must be less than or equal to 1");
        RuleFor(boundingBox => boundingBox.Width)
            .Must(width => width >= 0).WithMessage("{PropertyName} must be greater than or equal to 0")
            .Must(width => width <= 1).WithMessage("{PropertyName} must be less than or equal to 1");
        RuleFor(boundingBox => boundingBox.Height)
            .Must(height => height >= 0).WithMessage("{PropertyName} must be greater than or equal to 0")
            .Must(height => height <= 1).WithMessage("{PropertyName} must be less than or equal to 1");
    }
}