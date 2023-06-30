using FluentValidation;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services.Validating;

public sealed class ModelValidator : AbstractValidator<IModel>
{
    public ModelValidator(IValidator<IResolution> resolutionValidator)
    {
        RuleFor(model => model.Name).NotEmpty().MaximumLength(200);
        RuleFor(model => model.Resolution).SetValidator(resolutionValidator);
    }
}