using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain;
using SightKeeper.Domain.Screenshots;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Screenshotting;

public abstract class ScreenshotsDataAccess : ObservableDataAccess<Screenshot>, IDisposable
{
	public IObservable<Screenshot> Added => _added.AsObservable();
	public IObservable<Screenshot> Removed => _removed.AsObservable();

	public abstract Stream LoadImage(Screenshot screenshot);

	public Screenshot CreateScreenshot(ScreenshotsLibrary library, ReadOnlySpan2D<Rgba32> imageData, DateTimeOffset creationDate)
	{
		Vector2<ushort> resolution = new((ushort)imageData.Width, (ushort)imageData.Height);
		var screenshot = CreateScreenshot(library, creationDate, resolution);
		SaveScreenshotData(screenshot, imageData);
		_added.OnNext(screenshot);
		return screenshot;
	}

	protected virtual Screenshot CreateScreenshot(ScreenshotsLibrary library, DateTimeOffset creationDate, Vector2<ushort> resolution)
	{
		return library.CreateScreenshot(creationDate, resolution);
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