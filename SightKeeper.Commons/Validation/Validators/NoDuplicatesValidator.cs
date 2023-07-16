using FluentValidation;
using FluentValidation.Validators;

namespace SightKeeper.Commons.Validation.Validators;

public sealed class NoDuplicatesValidator<T, TProperty> : PropertyValidator<T, IEnumerable<TProperty>>
{
    public override bool IsValid(ValidationContext<T> context, IEnumerable<TProperty> value)
    {
        var duplicates = value
            .GroupBy(item => item)
            .Where(group => group.Count() > 1)
            .Select(group => group.First())
            .ToList();
        if (duplicates.Any())
        {
            context.MessageFormatter.AppendArgument("Duplicates", string.Join(", ", duplicates));
            return false;
        }
        return true;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "{PropertyName} contains duplicates: {Duplicates}";
    }

    public override string Name => "NoDuplicatesValidator";
}