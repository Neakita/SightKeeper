using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Decorators;

internal sealed class ImagesDataRemovingImageSet(ImageSet inner) : ImageSet, Decorator<ImageSet>
{
	public string Name
	{
		get => inner.Name;
		set => inner.Name = value;
	}

	public string Description
	{
		get => inner.Description;
		set => inner.Description = value;
	}

	public IReadOnlyList<ManagedImage> Images => inner.Images;
	public ImageSet Inner => inner;

	public ManagedImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		return inner.CreateImage(creationTimestamp, size);
	}

	public IReadOnlyList<ManagedImage> GetImagesRange(int index, int count)
	{
		return inner.GetImagesRange(index, count);
	}

	public void RemoveImageAt(int index)
	{
		var image = Images[index];
		DeleteData(image);
		inner.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		for (int i = 0; i < count; i++)
			DeleteDataAt(index + i);
		inner.RemoveImagesRange(index, count);
	}

	private void DeleteDataAt(int index)
	{
		var image = Images[index];
		DeleteData(image);
	}

	private static void DeleteData(ManagedImage image)
	{
		var deletableImageData = image.GetFirst<DeletableData>();
		deletableImageData.DeleteData();
	}
}