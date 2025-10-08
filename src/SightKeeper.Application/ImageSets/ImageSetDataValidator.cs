using FluentValidation;

namespace SightKeeper.Application.ImageSets;

internal sealed class ImageSetDataValidator : AbstractValidator<ImageSetData>
{
	public static ImageSetDataValidator Instance { get; } = new(); 

	public ImageSetDataValidator()
	{
		RuleFor(library => library.Name).NotEmpty();
	}
}