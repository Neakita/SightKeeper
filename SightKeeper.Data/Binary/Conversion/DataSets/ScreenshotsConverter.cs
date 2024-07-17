using System.Collections.Immutable;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class ScreenshotsConverter
{
	public ScreenshotsConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	internal ImmutableArray<SerializableScreenshot> Convert(
		IReadOnlyCollection<Screenshot> screenshots)
	{
		return screenshots.Select(Convert).ToImmutableArray();
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private SerializableScreenshot Convert(Screenshot screenshot)
	{
		return new SerializableScreenshot(_screenshotsDataAccess.GetId(screenshot), screenshot.CreationDate);
	}
}