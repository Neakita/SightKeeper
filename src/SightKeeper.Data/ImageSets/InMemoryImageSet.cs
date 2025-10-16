using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal sealed class InMemoryImageSet(ImageWrapper imageWrapper) : ImageSet, SettableInitialItems<ManagedImage>
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	public IReadOnlyList<ManagedImage> Images => _images;

	public ManagedImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		InMemoryImage inMemoryImage = new(Id.Create(), creationTimestamp, size);
		var wrappedImage = imageWrapper.Wrap(inMemoryImage);
		_images.Add(wrappedImage);
		return wrappedImage;
	}

	public IReadOnlyList<ManagedImage> GetImagesRange(int index, int count)
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

	public ManagedImage WrapAndInsert(ManagedImage image)
	{
		var index = _images.BinarySearch(image, ImageCreationTimestampComparer.Instance);
		Guard.IsLessThan(index, 0);
		index = ~index;
		var wrappedImage = imageWrapper.Wrap(image);
		_images.Insert(index, wrappedImage);
		return wrappedImage;
	}

	public void EnsureCapacity(int capacity)
	{
		_images.EnsureCapacity(capacity);
	}

	private readonly List<ManagedImage> _images = new();
}