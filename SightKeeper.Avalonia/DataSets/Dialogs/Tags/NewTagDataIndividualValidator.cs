using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

public sealed class NewTagDataIndividualValidator : AbstractValidator<NewTagData>
{
	public NewTagDataIndividualValidator(IReadOnlyCollection<NewTagData> siblingsCollection)
	{
		_siblingsCollection = siblingsCollection;
		RuleFor(data => data.Name)
			.NotEmpty()
			.Must(IsNameUnique).WithMessage("Name must be unique");
	}

	private readonly IReadOnlyCollection<NewTagData> _siblingsCollection;

	private bool IsNameUnique(NewTagData tagData, string name)
	{
		return _siblingsCollection.All(item => item.Name != name || item == tagData);
	}
}