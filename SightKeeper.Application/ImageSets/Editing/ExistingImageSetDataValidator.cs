using FluentValidation;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Editing;

public sealed class ExistingImageSetDataValidator : AbstractValidator<ImageSetData>
{
	public ExistingImageSetDataValidator(ImageSet library, IValidator<ImageSetData> baseValidator, ReadDataAccess<ImageSet> dataAccess)
	{
		_library = library;
		_dataAccess = dataAccess;
		Include(baseValidator);
		RuleFor(data => data.Name)
			.Must((_, name) => IsNameFree(name))
			.Unless(data => string.IsNullOrEmpty(data.Name))
			.WithMessage("Name must be unique");
	}

	private readonly ImageSet _library;
	private readonly ReadDataAccess<ImageSet> _dataAccess;

	private bool IsNameFree(string name)
	{
		return _dataAccess.Items.All(library => library == _library || library.Name != name);
	}
}