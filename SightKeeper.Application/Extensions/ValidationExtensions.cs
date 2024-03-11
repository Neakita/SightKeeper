using System.Numerics;
using FluentValidation;
using SightKeeper.Application.Validation;

namespace SightKeeper.Application.Extensions;

public static class ValidationExtensions
{
	public static IRuleBuilderOptions<TValidatable, IEnumerable<TProperty>> NoDuplicates<TValidatable, TProperty>(this IRuleBuilder<TValidatable, IEnumerable<TProperty>> ruleBuilder) =>
		ruleBuilder.SetValidator(new NoDuplicatesValidator<TValidatable, TProperty, TProperty>(property => property));
    
	public static IRuleBuilderOptions<TValidatable, IEnumerable<TProperty>> NoDuplicates<TValidatable, TProperty, TPropertyPredicate>(this IRuleBuilder<TValidatable, IEnumerable<TProperty>> ruleBuilder, Func<TProperty, TPropertyPredicate> predicate) =>
		ruleBuilder.SetValidator(new NoDuplicatesValidator<TValidatable, TProperty, TPropertyPredicate>(predicate));
    
	public static IRuleBuilderOptions<T, TProperty> MultiplierOf<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, TProperty multiplier) where TProperty : IModulusOperators<TProperty, TProperty, TProperty>, IEqualityOperators<TProperty, int, bool> =>
		ruleBuilder.SetValidator(new MultiplierOfValidator<T, TProperty>(multiplier));
    
	public static IRuleBuilderOptions<T, TProperty?> MultiplierOf<T, TProperty>(this IRuleBuilder<T, TProperty?> ruleBuilder, TProperty multiplier) where TProperty : struct, IModulusOperators<TProperty, TProperty, TProperty>, IEqualityOperators<TProperty, int, bool> =>
		ruleBuilder.SetValidator(new MultiplierOfValidator<T, TProperty>(multiplier));

	public static IRuleBuilderOptions<T, string> Contains<T>(this IRuleBuilder<T, string> ruleBuilder, string subString) =>
		ruleBuilder.SetValidator(new ContainsValidator<T>(subString));
}