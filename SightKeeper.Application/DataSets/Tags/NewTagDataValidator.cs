using FluentValidation;

namespace SightKeeper.Application.DataSets.Tags;

public sealed class NewTagDataValidator : AbstractValidator<NewTagData>
{
	public NewTagDataValidator(IReadOnlyCollection<NewTagData> siblingsCollection)
	{
		_siblingsCollection = siblingsCollection;
		RuleFor(data => data.Name)
			.NotEmpty()
			.Must(IsNameUnique).WithMessage("Name must be unique");
	}

	private readonly IReadOnlyCollection<NewTagData> _siblingsCollection;

	private bool IsNameUnique(NewTagData tagData, string name)
	{
		return _siblingsCollection
			.Where(item => item != tagData)
			.All(sibling => sibling.Name != name);
	}
}