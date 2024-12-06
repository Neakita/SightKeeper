using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using PackableScreenshotsLibrary = SightKeeper.Data.Binary.Model.PackableScreenshotsLibrary;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class ScreenshotsLibraryConverter
{
	public ScreenshotsLibraryConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public IEnumerable<PackableScreenshotsLibrary> ConvertScreenshotsLibraries(IEnumerable<ScreenshotsLibrary> packableLibraries)
	{
		return packableLibraries.Select(ConvertScreenshotsLibrary);
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private PackableScreenshotsLibrary ConvertScreenshotsLibrary(ScreenshotsLibrary packableLibrary) => new()
	{
		Name = packableLibrary.Name,
		Screenshots = ConvertScreenshots(packableLibrary.Screenshots).ToImmutableArray()
	};

	private IEnumerable<PackableScreenshot> ConvertScreenshots(IEnumerable<Screenshot> screenshots)
	{
		return screenshots.Select(ConvertScreenshot);
	}

	private PackableScreenshot ConvertScreenshot(Screenshot screenshot)
	{
		return new PackableScreenshot
		{
			Id = _screenshotsDataAccess.GetId(screenshot),
			CreationDate = screenshot.CreationDate,
			ImageSize = screenshot.ImageSize
		};
	}
}