using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Application;

public abstract class ScreenshotsDataAccess : ObservableDataAccess<Screenshot>, IDisposable
{
	public IObservable<Screenshot> Added => _added.AsObservable();
	public IObservable<Screenshot> Removed => _removed.AsObservable();

	public abstract Stream LoadImage(Screenshot screenshot);

	public Screenshot CreateScreenshot(
		ScreenshotsLibrary library,
		byte[] data,
		DateTimeOffset creationDate,
		Vector2<ushort> resolution)
	{
		var screenshot = library.CreateScreenshot(creationDate, resolution, out var removedScreenshots);
		foreach (var removedScreenshot in removedScreenshots)
			DeleteScreenshotData(removedScreenshot);
		SaveScreenshotData(screenshot, data);
		_added.OnNext(screenshot);
		return screenshot;
	}

	public Screenshot<TAsset> CreateScreenshot<TAsset>(
		ScreenshotsLibrary<TAsset> library,
		byte[] data,
		DateTimeOffset creationDate,
		Vector2<ushort> resolution)
		where TAsset : Asset
	{
		var screenshot = library.CreateScreenshot(creationDate, resolution, out var removedScreenshots);
		foreach (var removedScreenshot in removedScreenshots)
			DeleteScreenshotData(removedScreenshot);
		SaveScreenshotData(screenshot, data);
		_added.OnNext(screenshot);
		return screenshot;
	}

	public void DeleteScreenshot(Screenshot screenshot)
	{
		screenshot.DeleteFromLibrary();
		DeleteScreenshotData(screenshot);
		_removed.OnNext(screenshot);
	}

	public void Dispose()
	{
		_added.Dispose();
		_removed.Dispose();
	}

	protected abstract void SaveScreenshotData(Screenshot screenshot, byte[] data);
	protected abstract void DeleteScreenshotData(Screenshot screenshot);

	private readonly Subject<Screenshot> _added = new();
	private readonly Subject<Screenshot> _removed = new();
}