using System.Collections.Immutable;
using SightKeeper.Data.Conversion.DataSets;
using SightKeeper.Data.Services;

namespace SightKeeper.Data.Conversion;

internal sealed class AppDataConverter
{
	public AppDataConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public PackableAppData Convert(AppData data)
	{
		ConversionSession session = new();
		ScreenshotsLibraryConverter screenshotsLibrariesConverter = new(_screenshotsDataAccess);
		DataSetsConverter dataSetsConverter = new(session, _screenshotsDataAccess);
		return new PackableAppData
		{
			ScreenshotsLibraries = screenshotsLibrariesConverter.ConvertScreenshotsLibraries(data.ScreenshotsLibraries).ToImmutableArray(),
			DataSets = dataSetsConverter.ConvertDataSets(data.DataSets).ToImmutableArray(),
			ApplicationSettings = data.ApplicationSettings,
		};
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
}