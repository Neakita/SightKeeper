using FluentValidation;
using FluentValidation.Validators;

namespace SightKeeper.Application.Validation;

internal sealed class NoDuplicatesValidator<T, TProperty, TPropertyPredicate> : PropertyValidator<T, IEnumerable<TProperty>>
{
	public override string Name => "NoDuplicatesValidator";
    
	public NoDuplicatesValidator(Func<TProperty, TPropertyPredicate> predicate)
	{
		_predicate = predicate;
	}

	public override bool IsValid(ValidationContext<T> context, IEnumerable<TProperty> value)
	{
		var duplicates = value
			.GroupBy(_predicate)
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

	protected override string GetDefaultMessageTemplate(string errorCode) =>
		"{PropertyName} contains duplicates: {Duplicates}";

	private readonly Func<TProperty, TPropertyPredicate> _predicate;
}