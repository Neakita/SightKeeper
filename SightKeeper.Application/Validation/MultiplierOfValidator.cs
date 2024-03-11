using System.Numerics;
using FluentValidation;
using FluentValidation.Validators;

namespace SightKeeper.Application.Validation;

internal sealed class MultiplierOfValidator<T, TProperty> : PropertyValidator<T, TProperty> where TProperty : IModulusOperators<TProperty, TProperty, TProperty>, IEqualityOperators<TProperty, int, bool>
{
	public MultiplierOfValidator(TProperty multiplier)
	{
		_multiplier = multiplier;
	}

	public override bool IsValid(ValidationContext<T> context, TProperty value)
	{
		context.MessageFormatter.AppendArgument("Multiplier", _multiplier);
		return value % _multiplier == 0;
	}
    
	protected override string GetDefaultMessageTemplate(string errorCode)
	{
		return "{PropertyName} must be a multiple of {Multiplier}";
	}

	public override string Name => "MultiplierOfValidator";
    
	private readonly TProperty _multiplier;
}