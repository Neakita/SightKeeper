using FluentValidation;

namespace SightKeeper.Application.DataSets.Tags;

public sealed class TagDataValidator : AbstractValidator<TagData>
{
	public TagDataValidator(IReadOnlyCollection<TagData> siblingsCollection)
	{
		_siblingsCollection = siblingsCollection;
		RuleFor(data => data.Name)
			.NotEmpty()
			.Must(IsNameUnique).WithMessage("Name must be unique");
	}

	private readonly IReadOnlyCollection<TagData> _siblingsCollection;

	private bool IsNameUnique(TagData tagData, string name)
	{
		return _siblingsCollection
			.Where(item => item != tagData)
			.All(sibling => sibling.Name != name);
	}
}