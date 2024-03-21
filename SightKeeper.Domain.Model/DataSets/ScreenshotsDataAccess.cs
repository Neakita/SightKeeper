using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.Model.Extensions;

namespace SightKeeper.Domain.Model.DataSets;

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
		_screenshotAdded.OnNext(screenshot);
		SaveScreenshot(screenshot, new Image(data));
		return screenshot;
	}

	public virtual void Dispose()
	{
		_screenshotAdded.Dispose();
		_screenshotRemoved.Dispose();
		GC.SuppressFinalize(this);
	}

	protected abstract void SaveScreenshot(Screenshot screenshot, Image image);

	private readonly Subject<Screenshot> _screenshotAdded = new();
	private readonly Subject<Screenshot> _screenshotRemoved = new();

	private void ClearExceed(ScreenshotsLibrary library)
	{
		var removedScreenshots = library.ClearExceed();
		_screenshotRemoved.OnNextRange(removedScreenshots);
	}
}