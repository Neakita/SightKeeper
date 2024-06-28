using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Domain.Services;

public abstract class ClassifierScreenshotsDataAccess
{
	public abstract Image LoadImage(ClassifierScreenshot screenshot);

	public ClassifierScreenshot CreateScreenshot(ClassifierScreenshotsLibrary library, byte[] data)
	{
		var screenshot = library.CreateScreenshot();
		library.ClearExceed();
		SaveScreenshotData(screenshot, new Image(data));
		return screenshot;
	}

	public ClassifierScreenshot CreateScreenshot(ClassifierDataSet dataSet, byte[] data)
	{
		return CreateScreenshot(dataSet.Screenshots, data);
	}

	public void DeleteScreenshot(ClassifierScreenshot screenshot)
	{
		var library = screenshot.Library;
		library.DeleteScreenshot(screenshot);
		DeleteScreenshotData(screenshot);
	}

	protected abstract void SaveScreenshotData(ClassifierScreenshot screenshot, Image image);
	protected abstract void DeleteScreenshotData(ClassifierScreenshot screenshot);
}