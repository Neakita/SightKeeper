using FluentValidation;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application.ScreenshotsLibraries.Editing;

public sealed class ExistingScreenshotsLibraryDataValidator : AbstractValidator<ScreenshotsLibraryData>
{
	public ExistingScreenshotsLibraryDataValidator(ScreenshotsLibrary library, IValidator<ScreenshotsLibraryData> baseValidator, ReadDataAccess<ScreenshotsLibrary> dataAccess)
	{
		_library = library;
		_dataAccess = dataAccess;
		Include(baseValidator);
		RuleFor(data => data.Name)
			.Must((_, dataSetName) => IsNameFree(dataSetName))
			.Unless(data => string.IsNullOrEmpty(data.Name))
			.WithMessage("Name must be unique");
	}

	private readonly ScreenshotsLibrary _library;
	private readonly ReadDataAccess<ScreenshotsLibrary> _dataAccess;

	private bool IsNameFree(string name)
	{
		return _dataAccess.Items.All(library => library == _library || library.Name != name);
	}
}