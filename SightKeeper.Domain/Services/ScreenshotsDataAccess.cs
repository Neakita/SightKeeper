using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

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

	public Screenshot CreateScreenshot(DataSet dataSet, byte[] data)
	{
		return CreateScreenshot(dataSet.Screenshots, data);
	}

	public Screenshot<TAsset> CreateScreenshot<TAsset>(ScreenshotsLibrary<TAsset> library, byte[] data) where TAsset : Asset
	{
		var screenshot = library.CreateScreenshot();
		foreach (var removedScreenshot in library.ClearExceed())
			DeleteScreenshotData(removedScreenshot);
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