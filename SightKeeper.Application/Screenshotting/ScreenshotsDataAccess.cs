using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SixLabors.ImageSharp;

namespace SightKeeper.Application.Screenshotting;

public abstract class ScreenshotsDataAccess : ObservableDataAccess<Screenshot>, IDisposable
{
	public IObservable<Screenshot> Added => _added.AsObservable();
	public IObservable<Screenshot> Removed => _removed.AsObservable();

	public abstract Stream LoadImage(Screenshot screenshot);

	public Screenshot CreateScreenshot(
		ScreenshotsLibrary library,
		Image image,
		DateTimeOffset creationDate)
	{
		Vector2<ushort> resolution = new((ushort)image.Width, (ushort)image.Height);
		var screenshot = library.CreateScreenshot(creationDate, resolution, out var removedScreenshots);
		foreach (var removedScreenshot in removedScreenshots)
			DeleteScreenshotData(removedScreenshot);
		SaveScreenshotData(screenshot, image);
		_added.OnNext(screenshot);
		return screenshot;
	}

	public Screenshot<TAsset> CreateScreenshot<TAsset>(
		ScreenshotsLibrary<TAsset> library,
		Image image,
		DateTimeOffset creationDate)
		where TAsset : Asset
	{
		Vector2<ushort> resolution = new((ushort)image.Width, (ushort)image.Height);
		var screenshot = library.CreateScreenshot(creationDate, resolution, out var removedScreenshots);
		foreach (var removedScreenshot in removedScreenshots)
			DeleteScreenshotData(removedScreenshot);
		SaveScreenshotData(screenshot, image);
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

	protected abstract void SaveScreenshotData(Screenshot screenshot, Image image);
	protected abstract void DeleteScreenshotData(Screenshot screenshot);

	private readonly Subject<Screenshot> _added = new();
	private readonly Subject<Screenshot> _removed = new();
}