using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;

namespace SightKeeper.Data.ImageSets.Decorators;

internal sealed class ImagesDataRemovingImageSet(StorableImageSet inner) : StorableImageSet
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

	public IReadOnlyList<StorableImage> Images => inner.Images;

	public StorableImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		return inner.CreateImage(creationTimestamp, size);
	}

	public IReadOnlyList<StorableImage> GetImagesRange(int index, int count)
	{
		return inner.GetImagesRange(index, count);
	}

	public void RemoveImageAt(int index)
	{
		var image = Images[index];
		image.DeleteData();
		inner.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		for (int i = 0; i < count; i++)
			DeleteDataAt(index + i);
		inner.RemoveImagesRange(index, count);
	}

	public void Dispose()
	{
		inner.Dispose();
	}

	private void DeleteDataAt(int index)
	{
		var image = Images[index];
		image.DeleteData();
	}

	public StorableImage WrapAndInsertImage(StorableImage image)
	{
		return inner.WrapAndInsertImage(image);
	}
}