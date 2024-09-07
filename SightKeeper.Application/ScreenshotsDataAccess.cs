using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Application;

public abstract class ScreenshotsDataAccess
{
	public abstract byte[] LoadImage(Screenshot screenshot);

	public Screenshot CreateScreenshot(
		ScreenshotsLibrary library,
		byte[] data,
		DateTime creationDate,
		Vector2<ushort> resolution)
	{
		var screenshot = library.CreateScreenshot(creationDate, resolution, out var removedScreenshots);
		foreach (var removedScreenshot in removedScreenshots)
			DeleteScreenshotData(removedScreenshot);
		SaveScreenshotData(screenshot, data);
		return screenshot;
	}

	public Screenshot<TAsset> CreateScreenshot<TAsset>(
		ScreenshotsLibrary<TAsset> library,
		byte[] data,
		DateTime creationDate,
		Vector2<ushort> resolution)
		where TAsset : Asset
	{
		var screenshot = library.CreateScreenshot(creationDate, resolution, out var removedScreenshots);
		foreach (var removedScreenshot in removedScreenshots)
			DeleteScreenshotData(removedScreenshot);
		SaveScreenshotData(screenshot, data);
		return screenshot;
	}

	public void DeleteScreenshot(Screenshot screenshot)
	{
		screenshot.DeleteFromLibrary();
		DeleteScreenshotData(screenshot);
	}

	protected abstract void SaveScreenshotData(Screenshot screenshot, byte[] data);
	protected abstract void DeleteScreenshotData(Screenshot screenshot);
}