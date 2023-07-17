using System.Numerics;
using FluentValidation;
using SightKeeper.Commons.Validation.Validators;

namespace SightKeeper.Commons.Validation;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, IEnumerable<TItem>> NoDuplicates<T, TItem>(this IRuleBuilder<T, IEnumerable<TItem>> ruleBuilder) =>
        ruleBuilder.SetValidator(new NoDuplicatesValidator<T, TItem>());
    
    public static IRuleBuilderOptions<T, TProperty> MultiplierOf<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, TProperty multiplier) where TProperty : IModulusOperators<TProperty, TProperty, TProperty>, IEqualityOperators<TProperty, int, bool> =>
        ruleBuilder.SetValidator(new MultiplierOfValidator<T, TProperty>(multiplier));
    
    public static IRuleBuilderOptions<T, TProperty?> MultiplierOf<T, TProperty>(this IRuleBuilder<T, TProperty?> ruleBuilder, TProperty multiplier) where TProperty : struct, IModulusOperators<TProperty, TProperty, TProperty>, IEqualityOperators<TProperty, int, bool> =>
        ruleBuilder.SetValidator(new MultiplierOfValidator<T, TProperty>(multiplier));

    public static IRuleBuilderOptions<T, string> Contains<T>(this IRuleBuilder<T, string> ruleBuilder, string subString) =>
        ruleBuilder.SetValidator(new ContainsValidator<T>(subString));
}