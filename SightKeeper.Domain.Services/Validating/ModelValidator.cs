using FluentValidation;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Model;

namespace SightKeeper.Domain.Services.Validating;

public sealed class ModelValidator : AbstractValidator<IModel>
{
    public ModelValidator(IValidator<IResolution> resolutionValidator)
    {
        RuleFor(model => model.Name).NotEmpty().MaximumLength(200);
        RuleFor(model => model.Resolution).SetValidator(resolutionValidator);
    }
}