using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application.Annotating;

public sealed class Screenshoter
{
	public Screenshoter(ScreenCapture screenCapture, ScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenCapture = screenCapture;
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public Screenshot MakeScreenshot(DetectorDataSet dataSet)
	{
		return MakeScreenshot(dataSet.Screenshots, dataSet.Resolution, dataSet.Game);
	}

	private readonly ScreenCapture _screenCapture;
	private readonly ScreenshotsDataAccess _screenshotsDataAccess;

	private Screenshot MakeScreenshot(ScreenshotsLibrary library, ushort resolution, Game? game = null)
	{
		var image = _screenCapture.Capture(resolution, game);
		return _screenshotsDataAccess.CreateScreenshot(library, image);
	}
}