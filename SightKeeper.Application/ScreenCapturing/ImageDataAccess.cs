using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.HighPerformance;
using SightKeeper.Application.ImageSets;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.ScreenCapturing;

public abstract class ImageDataAccess : ObservableImageDataAccess, IDisposable
{
	public IObservable<Image> Added => _added.AsObservable();
	public IObservable<Image> Removed => _removed.AsObservable();

	public abstract Stream LoadImage(Image image);

	public Image CreateImage(ImageSet library, ReadOnlySpan2D<Rgba32> imageData, DateTimeOffset creationTimestamp)
	{
		Vector2<ushort> resolution = new((ushort)imageData.Width, (ushort)imageData.Height);
		var image = CreateImage(library, creationTimestamp, resolution);
		SaveImageData(image, imageData);
		_added.OnNext(image);
		return image;
	}

	public virtual void DeleteImage(ImageSet set, int index)
	{
		var image = set.Images[index];
		set.RemoveImageAt(index);
		DeleteImageData(image);
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

	private readonly Subject<Image> _added = new();
	private readonly Subject<Image> _removed = new();
}