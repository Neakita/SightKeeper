using FluentValidation;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services.Validating;

public sealed class ResolutionValidator : AbstractValidator<IResolution>
{
    public ResolutionValidator()
    {
        RuleFor(resolution => resolution.Width)
            .Must(width => width > 0).WithMessage("{PropertyName} must be greater than 0")
            .Must(width => width % 32 == 0).WithMessage("{PropertyName} must be a multiple of 32");
        RuleFor(resolution => resolution.Height)
            .Must(height => height > 0).WithMessage("{PropertyName} must be greater than 0")
            .Must(height => height % 32 == 0).WithMessage("{PropertyName} must be a multiple of 32");
    }
}