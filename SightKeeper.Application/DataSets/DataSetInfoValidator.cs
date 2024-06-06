using FluentValidation;
using SightKeeper.Application.Extensions;

namespace SightKeeper.Application.DataSets;

public sealed class DataSetInfoValidator : AbstractValidator<DataSetInfo>
{
    public DataSetInfoValidator()
    {
        RuleFor(changes => changes.Tags).NoDuplicates(tag => tag.Name);
    }
}