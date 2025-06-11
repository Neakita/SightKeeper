using FlakeId;
using MemoryPack;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model;

[MemoryPackable]
internal sealed partial class PackableImageSet : ImageSet
{
	public string Name { get; set; }
	public string Description { get; set; }
	[MemoryPackIgnore]
	public IReadOnlyList<Image> Images => _images;

	public PackableImageSet()
	{
		Name = string.Empty;
		Description = string.Empty;
		_images = new List<PackableImage>();
	}

	[MemoryPackConstructor]
	public PackableImageSet(string name, string description, List<PackableImage> images)
	{
		Name = name;
		Description = description;
		_images = images;
	}

	public Image CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		var id = Id.Create();
		PackableImage image = new(creationTimestamp, size, id);
		_images.Add(image);
		return image;
	}

	public IReadOnlyList<Image> GetImagesRange(int index, int count)
	{
		return _images.GetRange(index, count);
	}

	public void RemoveImage(Image image)
	{
		_images.Remove((PackableImage)image);
	}

	public void RemoveImageAt(int index)
	{
		_images.RemoveAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		_images.RemoveRange(index, count);
	}

	[MemoryPackInclude]
	private readonly List<PackableImage> _images;
}