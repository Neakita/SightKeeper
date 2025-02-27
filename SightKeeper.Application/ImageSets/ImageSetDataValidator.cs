using FluentValidation;

namespace SightKeeper.Application.ImageSets;

public sealed class ImageSetDataValidator : AbstractValidator<ImageSetData>
{
	public ImageSetDataValidator()
	{
		RuleFor(library => library.Name).NotEmpty();
	}
}