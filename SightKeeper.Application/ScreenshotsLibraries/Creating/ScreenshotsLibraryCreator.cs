using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application.ScreenshotsLibraries.Creating;

public sealed class ScreenshotsLibraryCreator
{
	public ScreenshotsLibraryCreator(WriteDataAccess<ScreenshotsLibrary> dataAccess)
	{
		_dataAccess = dataAccess;
	}

	public ScreenshotsLibrary Create(ScreenshotsLibraryData data)
	{
		ScreenshotsLibrary library = new()
		{
			Name = data.Name,
			Description = data.Description
		};
		_dataAccess.Add(library);
		return library;
	}

	private readonly WriteDataAccess<ScreenshotsLibrary> _dataAccess;
}