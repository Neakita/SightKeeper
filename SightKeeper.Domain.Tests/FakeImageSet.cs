using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests;

internal sealed class FakeImageSet : ImageSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public IReadOnlyList<DomainImage> Images => _images;

	public DomainImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		DomainImage image = new(creationTimestamp, size);
		_images.Add(image);
		return image;
	}

	public IReadOnlyList<DomainImage> GetImagesRange(int index, int count)
	{
		return _images.GetRange(index, count);
	}

	public void RemoveImage(DomainImage image)
	{
		_images.Remove(image);
	}

	public void RemoveImageAt(int index)
	{
		_images.RemoveAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		_images.RemoveRange(index, count);
	}

	private readonly List<DomainImage> _images = new();
}