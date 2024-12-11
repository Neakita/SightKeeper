using FluentValidation;

namespace SightKeeper.Application.ScreenshotsLibraries;

public sealed class ScreenshotsLibraryDataValidator : AbstractValidator<ScreenshotsLibraryData>
{
	public ScreenshotsLibraryDataValidator()
	{
		RuleFor(library => library.Name).NotEmpty();
	}
}