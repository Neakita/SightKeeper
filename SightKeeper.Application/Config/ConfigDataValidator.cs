using FluentValidation;
using SightKeeper.Commons.Validation;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Config;

public sealed class ConfigDataValidator : AbstractValidator<ConfigData>
{
    public ConfigDataValidator()
    {
        RuleFor(config => config.Name).NotEmpty();
        RuleFor(config => config.Content)
            .Contains(DetectorConfig.BatchPlaceholder)
            .Contains(DetectorConfig.SubdivisionsPlaceholder)
            .Contains(DetectorConfig.WidthPlaceholder)
            .Contains(DetectorConfig.HeightPlaceholder)
            .Contains(DetectorConfig.MaxBatchesPlaceholder)
            .Contains(DetectorConfig.Steps80Placeholder)
            .Contains(DetectorConfig.Steps90Placeholder)
            .Contains(DetectorConfig.ClassesCountPlaceholder);
    }
}