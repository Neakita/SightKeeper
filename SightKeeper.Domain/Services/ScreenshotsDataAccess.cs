using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Domain.Services;

public abstract class ScreenshotsDataAccess
{
	public abstract Image LoadImage(Screenshot screenshot);

	public Screenshot CreateScreenshot(ScreenshotsLibrary library, byte[] data)
	{
		var screenshot = library.CreateScreenshot();
		library.ClearExceed();
		SaveScreenshotData(screenshot, new Image(data));
		return screenshot;
	}

	public TScreenshot CreateScreenshot<TScreenshot>(ScreenshotsLibrary<TScreenshot> library, byte[] data) where TScreenshot : Screenshot
	{
		var screenshot = library.CreateScreenshot();
		library.ClearExceed();
		SaveScreenshotData(screenshot, new Image(data));
		return screenshot;
	}

	public void DeleteScreenshot(Screenshot screenshot)
	{
		screenshot.DeleteFromLibrary();
		DeleteScreenshotData(screenshot);
	}

	protected abstract void SaveScreenshotData(Screenshot screenshot, Image image);
	protected abstract void DeleteScreenshotData(Screenshot screenshot);
}