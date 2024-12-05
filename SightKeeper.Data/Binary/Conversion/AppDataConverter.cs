using SightKeeper.Data.Binary.Services;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class AppDataConverter
{
	public AppDataConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public PackableAppData Convert(AppData data)
	{
		ConversionSession session = new();
		MultiDataSetConverter dataSetConverter = new(_screenshotsDataAccess, session);
		var dataSets = dataSetConverter.Convert(data.DataSets);
		ProfileConverter profileConverter = new(session);
		var profiles = profileConverter.Convert(data.Profiles).ToImmutableArray();
		return new PackableAppData(games, dataSets, profiles, data.ApplicationSettings);
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
}