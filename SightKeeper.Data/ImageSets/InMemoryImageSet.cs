using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;

namespace SightKeeper.Data.ImageSets;

public sealed class InMemoryImageSet : StorableImageSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	public IReadOnlyList<StorableImage> Images => _images;

	public InMemoryImageSet(ImageWrapper imageWrapper)
	{
		_imageWrapper = imageWrapper;
		_images = new List<StorableImage>();
	}

	public StorableImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		InMemoryImage inMemoryImage = new(Id.Create(), creationTimestamp, size);
		var wrappedImage = _imageWrapper.Wrap(inMemoryImage);
		_images.Add(wrappedImage);
		return wrappedImage;
	}

	public IReadOnlyList<StorableImage> GetImagesRange(int index, int count)
	{
		return _images.GetRange(index, count);
	}

	public void RemoveImageAt(int index)
	{
		_images.RemoveAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		_images.RemoveRange(index, count);
	}

	public void Dispose()
	{
	}

	public void WrapAndInsertImage(StorableImage image)
	{
		var index = _images.BinarySearch(image, ImageCreationTimestampComparer<StorableImage>.Instance);
		Guard.IsLessThan(index, 0);
		index = ~index;
		var wrappedImage = _imageWrapper.Wrap(image);
		_images.Insert(index, wrappedImage);
	}

	internal void EnsureCapacity(int capacity)
	{
		_images.EnsureCapacity(capacity);
	}

	private readonly ImageWrapper _imageWrapper;
	private readonly List<StorableImage> _images;
}