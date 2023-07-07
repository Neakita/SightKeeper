using System.Numerics;
using FluentValidation;
using FluentValidation.Validators;

namespace SightKeeper.Commons.Validation.Validators;

public sealed class MultiplierOfValidator<T, TItem> : PropertyValidator<T, TItem> where TItem : IModulusOperators<TItem, TItem, TItem>, IEqualityOperators<TItem, int, bool>
{
    public MultiplierOfValidator(TItem multiplier)
    {
        _multiplier = multiplier;
    }

    public override bool IsValid(ValidationContext<T> context, TItem value)
    {
        context.MessageFormatter.AppendArgument("Multiplier", _multiplier);
        return value % _multiplier == 0;
    }
    
    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "{PropertyName} must be a multiple of {Multiplier}";
    }

    public override string Name => "MultiplierOfValidator";
    
    private readonly TItem _multiplier;
}