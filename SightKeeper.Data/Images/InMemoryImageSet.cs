using FlakeId;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

internal sealed class InMemoryImageSet : ImageSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public IReadOnlyList<Image> Images => _images;

	public InMemoryImageSet(ImageWrapper imageWrapper, int initialImagesCapacity = 0)
	{
		_imageWrapper = imageWrapper;
		_images = new List<Image>(initialImagesCapacity);
	}

	public Image CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		InMemoryImage inMemoryImage = new(Id.Create(), creationTimestamp, size);
		var wrappedImage = _imageWrapper.Wrap(inMemoryImage);
		_images.Add(wrappedImage);
		return wrappedImage;
	}

	public IReadOnlyList<Image> GetImagesRange(int index, int count)
	{
		return _images.GetRange(index, count);
	}

	public int IndexOf(Image image)
	{
		// images are ordered by creation timestamp, so binary search is possible and highly preferable,
		// images list can contain thousands of images.
		var index = _images.BinarySearch((InMemoryImage)image, ImageCreationTimestampComparer);
		if (index < 0)
			return -1;
		return index;
	}

	public void RemoveImageAt(int index)
	{
		_images.RemoveAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		_images.RemoveRange(index, count);
	}

	internal void AddImage(InMemoryImage image)
	{
		var wrappedImage = _imageWrapper.Wrap(image);
		_images.Add(wrappedImage);
	}

	private static readonly Comparer<Image> ImageCreationTimestampComparer =
		Comparer<Image>.Create((x, y) => x.CreationTimestamp.CompareTo(y.CreationTimestamp));

	private readonly ImageWrapper _imageWrapper;
	private readonly List<Image> _images;
}