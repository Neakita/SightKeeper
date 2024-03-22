using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.Extensions;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Domain.Services;

public abstract class ScreenshotsDataAccess : IDisposable
{
	public IObservable<Screenshot> ScreenshotAdded => _screenshotAdded.AsObservable();
	public IObservable<Screenshot> ScreenshotRemoved => _screenshotRemoved.AsObservable();

	public abstract Image LoadImage(Screenshot screenshot);
	public abstract IEnumerable<(Screenshot screenshot, Image image)> LoadImages(IEnumerable<Screenshot> screenshots, bool ordered = false);
	public abstract IEnumerable<(Screenshot screenshot, Image image)> LoadImages(IReadOnlyCollection<Screenshot> screenshots, bool ordered = false);
	public abstract IEnumerable<(Screenshot screenshot, Image image)> LoadImages(DataSet dataSet);

	public Screenshot CreateScreenshot(ScreenshotsLibrary library, byte[] data)
	{
		var screenshot = library.CreateScreenshot();
		ClearExceed(library);
		SaveScreenshotData(screenshot, new Image(data));
		_screenshotAdded.OnNext(screenshot);
		return screenshot;
	}

	public void DeleteScreenshot(Screenshot screenshot)
	{
		screenshot.Library.DeleteScreenshot(screenshot);
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

	private readonly Subject<Screenshot> _screenshotAdded = new();
	private readonly Subject<Screenshot> _screenshotRemoved = new();

	private void ClearExceed(ScreenshotsLibrary library)
	{
		var removedScreenshots = library.ClearExceed();
		_screenshotRemoved.OnNextRange(removedScreenshots);
	}
}