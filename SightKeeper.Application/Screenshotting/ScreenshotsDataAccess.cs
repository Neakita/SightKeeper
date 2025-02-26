using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Screenshotting;

public abstract class ScreenshotsDataAccess : ObservableScreenshotsDataAccess, IDisposable
{
	public IObservable<(ScreenshotsLibrary library, Screenshot screenshot)> Added => _added.AsObservable();
	public IObservable<(ScreenshotsLibrary library, Screenshot screenshot)> Removed => _removed.AsObservable();

	public abstract Stream LoadImage(Screenshot screenshot);

	public Screenshot CreateScreenshot(ScreenshotsLibrary library, ReadOnlySpan2D<Rgba32> imageData, DateTimeOffset creationTimestamp)
	{
		Vector2<ushort> resolution = new((ushort)imageData.Width, (ushort)imageData.Height);
		var screenshot = CreateScreenshot(library, creationTimestamp, resolution);
		SaveScreenshotData(screenshot, imageData);
		_added.OnNext((library, screenshot));
		return screenshot;
	}

	public virtual void DeleteScreenshot(ScreenshotsLibrary library, int index)
	{
		var screenshot = library.Screenshots[index];
		library.RemoveScreenshotAt(index);
		DeleteScreenshotData(screenshot);
	}

	public void Dispose()
	{
		_added.Dispose();
		_removed.Dispose();
	}

	protected virtual Screenshot CreateScreenshot(ScreenshotsLibrary library, DateTimeOffset creationTimestamp, Vector2<ushort> resolution)
	{
		return library.CreateScreenshot(creationTimestamp, resolution);
	}

	protected abstract void SaveScreenshotData(Screenshot screenshot, ReadOnlySpan2D<Rgba32> image);
	protected abstract void DeleteScreenshotData(Screenshot screenshot);

	private readonly Subject<(ScreenshotsLibrary library, Screenshot screenshot)> _added = new();
	private readonly Subject<(ScreenshotsLibrary library, Screenshot screenshot)> _removed = new();
}