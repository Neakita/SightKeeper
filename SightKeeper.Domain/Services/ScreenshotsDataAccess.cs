using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Services;

public abstract class ScreenshotsDataAccess : IDisposable
{
	public IObservable<(ScreenshotsLibrary library, Screenshot screenshot)> ScreenshotAdded => _screenshotAdded.AsObservable();
	public IObservable<Screenshot> ScreenshotRemoved => _screenshotRemoved.AsObservable();

	public abstract Image LoadImage(Screenshot screenshot);
	public abstract IEnumerable<(Screenshot screenshot, Image image)> LoadImages(IEnumerable<Screenshot> screenshots, bool ordered = false);
	public abstract IEnumerable<(Screenshot screenshot, Image image)> LoadImages(IReadOnlyCollection<Screenshot> screenshots, bool ordered = false);
	public abstract IEnumerable<(Screenshot screenshot, Image image)> LoadImages(DetectorDataSet dataSet);

	public ScreenshotsDataAccess(ObjectsLookupper objectsLookupper)
	{
		_objectsLookupper = objectsLookupper;
	}

	public Screenshot CreateScreenshot(ScreenshotsLibrary library, byte[] data)
	{
		var screenshot = library.CreateScreenshot();
		ClearExceed(library);
		SaveScreenshotData(screenshot, new Image(data));
		_screenshotAdded.OnNext((library, screenshot));
		return screenshot;
	}

	public void DeleteScreenshot(Screenshot screenshot)
	{
		var library = _objectsLookupper.GetLibrary(screenshot);
		library.DeleteScreenshot(screenshot);
		DeleteScreenshotData(screenshot);
		_screenshotRemoved.OnNext(screenshot);
	}

	public virtual void Dispose()
	{
		_screenshotAdded.Dispose();
		_screenshotRemoved.Dispose();
		GC.SuppressFinalize(this);
	}

	protected abstract void SaveScreenshotData(Screenshot screenshot, Image image);
	protected abstract void DeleteScreenshotData(Screenshot screenshot);

	private readonly ObjectsLookupper _objectsLookupper;
	private readonly Subject<(ScreenshotsLibrary, Screenshot)> _screenshotAdded = new();
	private readonly Subject<Screenshot> _screenshotRemoved = new();

	private void ClearExceed(ScreenshotsLibrary library)
	{
		var removedScreenshots = library.ClearExceed();
		foreach (var removedScreenshot in removedScreenshots)
			_screenshotRemoved.OnNext(removedScreenshot);
	}
}