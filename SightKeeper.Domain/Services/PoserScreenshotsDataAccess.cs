using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Services;

public abstract class PoserScreenshotsDataAccess
{
	public abstract Image LoadImage(PoserScreenshot screenshot);

	public PoserScreenshot CreateScreenshot(PoserScreenshotsLibrary library, byte[] data)
	{
		var screenshot = library.CreateScreenshot();
		library.ClearExceed();
		SaveScreenshotData(screenshot, new Image(data));
		return screenshot;
	}

	public PoserScreenshot CreateScreenshot(PoserDataSet dataSet, byte[] data)
	{
		return CreateScreenshot(dataSet.Screenshots, data);
	}

	public void DeleteScreenshot(PoserScreenshot screenshot)
	{
		var library = screenshot.Library;
		library.DeleteScreenshot(screenshot);
		DeleteScreenshotData(screenshot);
	}

	protected abstract void SaveScreenshotData(PoserScreenshot screenshot, Image image);
	protected abstract void DeleteScreenshotData(PoserScreenshot screenshot);
}