using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Application;

public abstract class ScreenshotsDataAccess
{
	public abstract byte[] LoadImage(Screenshot screenshot);

	public Screenshot CreateScreenshot(ScreenshotsLibrary library, byte[] data, DateTime creationDate)
	{
		throw new NotImplementedException();
		// var screenshot = library.CreateScreenshot(creationDate, out var removedScreenshots);
		// foreach (var removedScreenshot in removedScreenshots)
		// 	DeleteScreenshotData(removedScreenshot);
		// SaveScreenshotData(screenshot, data);
		// return screenshot;
	}

	public Screenshot CreateScreenshot(ScreenshotsLibrary library, byte[] data)
	{
		return CreateScreenshot(library, data, DateTime.Now);
	}

	public Screenshot CreateScreenshot(DataSet dataSet, byte[] data)
	{
		return CreateScreenshot(dataSet.Screenshots, data);
	}

	public Screenshot<TAsset> CreateScreenshot<TAsset>(ScreenshotsLibrary<TAsset> library, byte[] data, DateTime creationDate) where TAsset : Asset
	{
		throw new NotImplementedException();
		// var screenshot = library.CreateScreenshot(creationDate, out var removedScreenshots);
		// foreach (var removedScreenshot in removedScreenshots)
		// 	DeleteScreenshotData(removedScreenshot);
		// SaveScreenshotData(screenshot, data);
		// return screenshot;
	}

	public Screenshot<TAsset> CreateScreenshot<TAsset>(ScreenshotsLibrary<TAsset> library, byte[] data) where TAsset : Asset
	{
		return CreateScreenshot(library, data, DateTime.Now);
	}

	public void DeleteScreenshot(Screenshot screenshot)
	{
		screenshot.DeleteFromLibrary();
		DeleteScreenshotData(screenshot);
	}

	protected abstract void SaveScreenshotData(Screenshot screenshot, byte[] data);
	protected abstract void DeleteScreenshotData(Screenshot screenshot);
}