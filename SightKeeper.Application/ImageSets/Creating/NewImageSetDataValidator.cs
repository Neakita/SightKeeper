using FluentValidation;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Creating;

public sealed class NewImageSetDataValidator : AbstractValidator<ImageSetData>
{
	public NewImageSetDataValidator(
		IValidator<ImageSetData> imageSetDataValidator,
		ReadDataAccess<ImageSet> imageSetDataAccess)
	{
		_imageSetDataAccess = imageSetDataAccess;
		Include(imageSetDataValidator);
		RuleFor(data => data.Name)
			.Must((_, name) => IsNameFree(name))
			.Unless(data => string.IsNullOrEmpty(data.Name))
			.WithMessage("Name must be unique");
	}

	private readonly ReadDataAccess<ImageSet> _imageSetDataAccess;

	private bool IsNameFree(string name)
	{
		return _imageSetDataAccess.Items.All(dataSet => dataSet.Name != name);
	}
}