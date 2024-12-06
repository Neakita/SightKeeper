using System.Collections.Immutable;
using SightKeeper.Data.Binary.Conversion.DataSets;
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
		DataSetsConverter dataSetsConverter = new(session, _screenshotsDataAccess);
		return new PackableAppData
		{
			DataSets = dataSetsConverter.ConvertDataSets(data.DataSets).ToImmutableArray(),
			ApplicationSettings = data.ApplicationSettings
		};
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
}