using FluentValidation;
using SightKeeper.Application.Misc;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Creating;

internal sealed class NewImageSetDataValidator : AbstractValidator<ImageSetData>
{
	public NewImageSetDataValidator(ReadRepository<ImageSet> imageSetRepository)
	{
		_imageSetRepository = imageSetRepository;
		Include(ImageSetDataValidator.Instance);
		RuleFor(data => data.Name)
			.Must((_, name) => IsNameFree(name))
			.Unless(data => string.IsNullOrEmpty(data.Name))
			.WithMessage("Name must be unique");
	}

	private readonly ReadRepository<ImageSet> _imageSetRepository;

	private bool IsNameFree(string name)
	{
		return _imageSetRepository.Items.All(dataSet => dataSet.Name != name);
	}
}