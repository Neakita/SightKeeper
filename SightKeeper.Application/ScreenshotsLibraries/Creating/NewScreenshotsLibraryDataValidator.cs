using FluentValidation;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenshotsLibraries.Creating;

public sealed class NewScreenshotsLibraryDataValidator : AbstractValidator<ScreenshotsLibraryData>
{
	public NewScreenshotsLibraryDataValidator(
		IValidator<ScreenshotsLibraryData> screenshotsLibraryDataValidator,
		ReadDataAccess<ImageSet> screenshotsLibrariesDataAccess)
	{
		_screenshotsLibrariesDataAccess = screenshotsLibrariesDataAccess;
		Include(screenshotsLibraryDataValidator);
		RuleFor(data => data.Name)
			.Must((_, dataSetName) => IsNameFree(dataSetName))
			.Unless(data => string.IsNullOrEmpty(data.Name))
			.WithMessage("Name must be unique");
	}

	private readonly ReadDataAccess<ImageSet> _screenshotsLibrariesDataAccess;

	private bool IsNameFree(string name)
	{
		return _screenshotsLibrariesDataAccess.Items.All(dataSet => dataSet.Name != name);
	}
}