using FluentValidation;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Editing;

public sealed class ExistingImageSetDataValidator : AbstractValidator<ImageSetData>
{
	public ExistingImageSetDataValidator(ImageSet library, IValidator<ImageSetData> baseValidator, ReadRepository<ImageSet> repository)
	{
		_library = library;
		_repository = repository;
		Include(baseValidator);
		RuleFor(data => data.Name)
			.Must((_, name) => IsNameFree(name))
			.Unless(data => string.IsNullOrEmpty(data.Name))
			.WithMessage("Name must be unique");
	}

	private readonly ImageSet _library;
	private readonly ReadRepository<ImageSet> _repository;

	private bool IsNameFree(string name)
	{
		return _repository.Items.All(library => library == _library || library.Name != name);
	}
}