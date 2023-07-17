using FluentValidation;
using FluentValidation.Validators;

namespace SightKeeper.Commons.Validation.Validators;

public sealed class ContainsValidator<T> : PropertyValidator<T, string>
{
    public override string Name => "ContainsValidator";
    
    public ContainsValidator(string subString)
    {
        _subString = subString;
    }

    public override bool IsValid(ValidationContext<T> context, string value)
    {
        var isValid = value.Contains(_subString);
        if (!isValid)
            context.MessageFormatter.AppendArgument("SubString", _subString);
        return isValid;
    }

    protected override string GetDefaultMessageTemplate(string errorCode) => "{PropertyName} must contain \"{SubString}\"";

    private readonly string _subString;
}