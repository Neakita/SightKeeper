using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Services;

public abstract class DetectorScreenshotsDataAccess
{
	public abstract Image LoadImage(DetectorScreenshot screenshot);

	public DetectorScreenshot CreateScreenshot(DetectorScreenshotsLibrary library, byte[] data)
	{
		var screenshot = library.CreateScreenshot();
		library.ClearExceed();
		SaveScreenshotData(screenshot, new Image(data));
		return screenshot;
	}

	public DetectorScreenshot CreateScreenshot(DetectorDataSet dataSet, byte[] data)
	{
		return CreateScreenshot(dataSet.Screenshots, data);
	}

	public void DeleteScreenshot(DetectorScreenshot screenshot)
	{
		var library = screenshot.Library;
		library.DeleteScreenshot(screenshot);
		DeleteScreenshotData(screenshot);
	}

	protected abstract void SaveScreenshotData(DetectorScreenshot screenshot, Image image);
	protected abstract void DeleteScreenshotData(DetectorScreenshot screenshot);
}