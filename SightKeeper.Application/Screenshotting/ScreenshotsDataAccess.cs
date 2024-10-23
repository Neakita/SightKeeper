using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Screenshotting;

public abstract class ScreenshotsDataAccess : ObservableDataAccess<Screenshot>, IDisposable
{
	public IObservable<Screenshot> Added => _added.AsObservable();
	public IObservable<Screenshot> Removed => _removed.AsObservable();

	public abstract Stream LoadImage(Screenshot screenshot);

	public Screenshot CreateScreenshot(
		ScreenshotsLibrary library,
		ReadOnlySpan2D<Rgba32> imageData,
		DateTimeOffset creationDate)
	{
		Vector2<ushort> resolution = new((ushort)imageData.Width, (ushort)imageData.Height);
		var screenshot = library.CreateScreenshot(creationDate, resolution, out var removedScreenshots);
		foreach (var removedScreenshot in removedScreenshots)
		{
			DeleteScreenshotData(removedScreenshot);
			_removed.OnNext(removedScreenshot);
		}
		SaveScreenshotData(screenshot, imageData);
		_added.OnNext(screenshot);
		return screenshot;
	}

	public Screenshot<TAsset> CreateScreenshot<TAsset>(
		ScreenshotsLibrary<TAsset> library,
		ReadOnlySpan2D<Rgba32> imageData,
		DateTimeOffset creationDate)
		where TAsset : Asset
	{
		Vector2<ushort> resolution = new((ushort)imageData.Width, (ushort)imageData.Height);
		var screenshot = library.CreateScreenshot(creationDate, resolution, out var removedScreenshots);
		foreach (var removedScreenshot in removedScreenshots)
			DeleteScreenshotData(removedScreenshot);
		SaveScreenshotData(screenshot, imageData);
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

	protected abstract void SaveScreenshotData(Screenshot screenshot, ReadOnlySpan2D<Rgba32> image);
	protected abstract void DeleteScreenshotData(Screenshot screenshot);

	private readonly Subject<Screenshot> _added = new();
	private readonly Subject<Screenshot> _removed = new();
}