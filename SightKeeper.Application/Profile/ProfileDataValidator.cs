using FluentValidation;
using SightKeeper.Commons.Validation;

namespace SightKeeper.Application;

public sealed class ProfileDataValidator : AbstractValidator<ProfileData>
{
    public ProfileDataValidator()
    {
        RuleFor(data => data.Name)
            .NotEmpty();
        RuleFor(data => data.DetectionThreshold)
            .ExclusiveBetween(0, 1);
        RuleFor(data => data.MouseSensitivity)
            .GreaterThan(0);
        RuleFor(data => data.Weights)
            .NotNull();
        RuleForEach(data => data.ItemClasses)
            .Must((data, profileItemClass) => profileItemClass.ItemClass.DataSet == data.Weights!.Library.DataSet)
            .When(data => data.Weights != null);
        RuleFor(data => data.ItemClasses).NoDuplicates();
    }
}