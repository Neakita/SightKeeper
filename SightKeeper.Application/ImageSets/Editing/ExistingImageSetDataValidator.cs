using FluentValidation;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Editing;

public sealed class ExistingImageSetDataValidator : AbstractValidator<ExistingImageSetData>
{
	public ExistingImageSetDataValidator(ReadRepository<DomainImageSet> repository)
	{
		_repository = repository;
		Include(ImageSetDataValidator.Instance);
		RuleFor(data => data.Name)
			.Must((data, name) => IsNameFree(data.Set, name))
			.Unless(data => string.IsNullOrEmpty(data.Name))
			.WithMessage("Name must be unique");
	}

	private readonly ReadRepository<DomainImageSet> _repository;

	private bool IsNameFree(DomainImageSet subjectSet, string name)
	{
		return _repository.Items.All(storedSet => storedSet == subjectSet || storedSet.Name != name);
	}
}