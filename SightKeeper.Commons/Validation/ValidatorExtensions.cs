using System.Numerics;
using FluentValidation;
using SightKeeper.Commons.Validation.Validators;

namespace SightKeeper.Commons.Validation;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, IEnumerable<TItem>> NoDuplicates<T, TItem>(this IRuleBuilder<T, IEnumerable<TItem>> ruleBuilder) =>
        ruleBuilder.SetValidator(new NoDuplicatesValidator<T, TItem>());
    
    public static IRuleBuilderOptions<T, TItem> MultiplierOf<T, TItem>(this IRuleBuilder<T, TItem> ruleBuilder, TItem multiplier) where TItem : IModulusOperators<TItem, TItem, TItem>, IEqualityOperators<TItem, int, bool> =>
        ruleBuilder.SetValidator(new MultiplierOfValidator<T, TItem>(multiplier));
}