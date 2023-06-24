using FluentValidation;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services.Validating;

public sealed class ResolutionValidator : AbstractValidator<IResolution>
{
    public ResolutionValidator()
    {
        RuleFor(resolution => resolution.Width)
            .Must(width => width > 0).WithMessage("{PropertyName} must be greater than 0")
            .Must(width => width % 32 == 0).WithMessage("{PropertyName} must be a multiple of 32")
            .Must(width => width <= 32000).WithMessage("{PropertyName} must be less than or equal to 32000");
        RuleFor(resolution => resolution.Height)
            .Must(height => height > 0).WithMessage("{PropertyName} must be greater than 0")
            .Must(height => height % 32 == 0).WithMessage("{PropertyName} must be a multiple of 32")
            .Must(height => height <= 32000).WithMessage("{PropertyName} must be less than or equal to 32000");
    }
}