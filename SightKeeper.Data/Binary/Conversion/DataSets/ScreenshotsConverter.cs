using System.Collections.Immutable;
using SightKeeper.Data.Binary.Services;
using Screenshot = SightKeeper.Data.Binary.DataSets.Screenshot;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class ScreenshotsConverter
{
	public ScreenshotsConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	internal ImmutableArray<Screenshot> Convert(
		IReadOnlyCollection<Domain.Model.DataSets.Screenshots.Screenshot> screenshots)
	{
		return screenshots.Select(Convert).ToImmutableArray();
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private Screenshot Convert(Domain.Model.DataSets.Screenshots.Screenshot screenshot)
	{
		return new Screenshot(_screenshotsDataAccess.GetId(screenshot), screenshot.CreationDate);
	}
}