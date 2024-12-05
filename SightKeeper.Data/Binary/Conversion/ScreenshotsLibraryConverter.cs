using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.Model.Screenshots;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class ScreenshotsLibraryConverter
{
	public ScreenshotsLibraryConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public PackableScreenshotsLibrary ConvertScreenshotsLibrary(ScreenshotsLibrary library)
	{
		return new PackableScreenshotsLibrary(library.Name, ConvertScreenshots(library.Screenshots));
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private ImmutableArray<Id> ConvertScreenshots(IReadOnlyCollection<Screenshot> screenshots)
	{
		var builder = ImmutableArray.CreateBuilder<Id>(screenshots.Count);
		foreach (var screenshot in screenshots)
		{
			var screenshotId = _screenshotsDataAccess.GetId(screenshot);
			builder.Add(screenshotId);
		}

		return builder.DrainToImmutable();
	}
}