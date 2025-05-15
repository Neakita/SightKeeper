using FluentValidation;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Creating;

public sealed class NewImageSetDataValidator : AbstractValidator<ImageSetData>
{
	public NewImageSetDataValidator(
		IValidator<ImageSetData> imageSetDataValidator,
		ReadRepository<ImageSet> imageSetRepository)
	{
		_imageSetRepository = imageSetRepository;
		Include(imageSetDataValidator);
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