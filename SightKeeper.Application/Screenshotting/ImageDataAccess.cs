using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Screenshotting;

public abstract class ImageDataAccess : ObservableScreenshotsDataAccess, IDisposable
{
	public IObservable<(ImageSet library, Image screenshot)> Added => _added.AsObservable();
	public IObservable<(ImageSet library, Image screenshot)> Removed => _removed.AsObservable();

	public abstract Stream LoadImage(Image image);

	public Image CreateScreenshot(ImageSet library, ReadOnlySpan2D<Rgba32> imageData, DateTimeOffset creationTimestamp)
	{
		Vector2<ushort> resolution = new((ushort)imageData.Width, (ushort)imageData.Height);
		var screenshot = CreateImage(library, creationTimestamp, resolution);
		SaveImageData(screenshot, imageData);
		_added.OnNext((library, screenshot));
		return screenshot;
	}

	public virtual void DeleteImage(ImageSet set, int index)
	{
		var screenshot = set.Images[index];
		set.RemoveImageAt(index);
		DeleteImageData(screenshot);
	}

	public void Dispose()
	{
		_added.Dispose();
		_removed.Dispose();
	}

	protected virtual Image CreateImage(ImageSet set, DateTimeOffset creationTimestamp, Vector2<ushort> resolution)
	{
		return set.CreateImage(creationTimestamp, resolution);
	}

	protected abstract void SaveImageData(Image image, ReadOnlySpan2D<Rgba32> data);
	protected abstract void DeleteImageData(Image image);

	private readonly Subject<(ImageSet library, Image screenshot)> _added = new();
	private readonly Subject<(ImageSet library, Image screenshot)> _removed = new();
}