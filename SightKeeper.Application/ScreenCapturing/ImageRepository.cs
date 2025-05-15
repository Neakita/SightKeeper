using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.HighPerformance;
using SightKeeper.Application.ImageSets;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.ScreenCapturing;

public class ImageRepository : ObservableImageRepository, IDisposable
{
	public IObservable<Image> Added => _added.AsObservable();
	public IObservable<Image> Removed => _removed.AsObservable();
	public IObservable<ImagesRange> ImagesDeleted => _imagesDeleted.AsObservable();

	public Image CreateImage(ImageSet library, ReadOnlySpan2D<Rgba32> imageData, DateTimeOffset creationTimestamp)
	{
		Vector2<ushort> resolution = new((ushort)imageData.Width, (ushort)imageData.Height);
		var image = CreateImage(library, creationTimestamp, resolution);
		_writeImageDataAccess.SaveImageData(image, imageData);
		_added.OnNext(image);
		return image;
	}

	public void DeleteImage(ImageSet set, int index)
	{
		DeleteImagesRange(set, index, 1);
	}

	public virtual void DeleteImagesRange(ImageSet set, int index, int count)
	{
		var images = set.GetImagesRange(index, count);
		set.RemoveImagesRange(index, count);
		foreach (var image in images)
		{
			_writeImageDataAccess.DeleteImageData(image);
			_removed.OnNext(image);
		}
		ImagesRange imagesRange = new()
		{
			Set = set,
			Images = images,
			Range = Range.FromCount(index, count)
		};
		_imagesDeleted.OnNext(imagesRange);
	}

	public void Dispose()
	{
		_added.Dispose();
		_removed.Dispose();
		_imagesDeleted.Dispose();
	}

	protected ImageRepository(WriteImageDataAccess writeImageDataAccess)
	{
		_writeImageDataAccess = writeImageDataAccess;
	}

	protected virtual Image CreateImage(ImageSet set, DateTimeOffset creationTimestamp, Vector2<ushort> resolution)
	{
		return set.CreateImage(creationTimestamp, resolution);
	}

	private readonly WriteImageDataAccess _writeImageDataAccess;
	private readonly Subject<Image> _added = new();
	private readonly Subject<Image> _removed = new();
	private readonly Subject<ImagesRange> _imagesDeleted = new();
}