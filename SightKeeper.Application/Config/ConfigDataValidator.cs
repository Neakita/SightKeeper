using FluentValidation;

namespace SightKeeper.Application.Config;

public sealed class ConfigDataValidator : AbstractValidator<ConfigData>
{
    public ConfigDataValidator()
    {
        RuleFor(config => config.Name).NotEmpty();
        RuleFor(config => config.Content).NotEmpty();
    }
}