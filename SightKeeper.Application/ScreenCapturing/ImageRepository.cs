using CommunityToolkit.HighPerformance;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.ScreenCapturing;

public class ImageRepository : ImageSaver<Rgba32>
{
	public ImageRepository(WriteImageDataAccess writeImageDataAccess)
	{
		_writeImageDataAccess = writeImageDataAccess;
	}

	public void SaveImage(ImageSet set, ReadOnlySpan2D<Rgba32> imageData, DateTimeOffset creationTimestamp)
	{
		Vector2<ushort> resolution = new((ushort)imageData.Width, (ushort)imageData.Height);
		var image = set.CreateImage(creationTimestamp, resolution);
		_writeImageDataAccess.SaveImageData(image, imageData);
	}

	public void DeleteImage(DomainImageSet set, int index)
	{
		DeleteImagesRange(set, index, 1);
	}

	public virtual void DeleteImagesRange(DomainImageSet set, int index, int count)
	{
		var images = set.GetImagesRange(index, count);
		set.RemoveImagesRange(index, count);
		foreach (var image in images)
			_writeImageDataAccess.DeleteImageData(image);
	}

	private readonly WriteImageDataAccess _writeImageDataAccess;
}