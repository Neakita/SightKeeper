using FluentValidation;
using FluentValidation.Validators;

namespace SightKeeper.Application.Validation;

internal sealed class ContainsValidator<T> : PropertyValidator<T, string>
{
	public override string Name => "ContainsValidator";
    
	public ContainsValidator(string subString)
	{
		_subString = subString;
	}

	public override bool IsValid(ValidationContext<T> context, string value)
	{
		context.MessageFormatter.AppendArgument("SubString", _subString);
		return value.Contains(_subString);
	}

	protected override string GetDefaultMessageTemplate(string errorCode) => "{PropertyName} must contain \"{SubString}\"";

	private readonly string _subString;
}