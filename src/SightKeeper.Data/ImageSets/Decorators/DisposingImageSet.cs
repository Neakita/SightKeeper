using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;

namespace SightKeeper.Data.ImageSets.Decorators;

internal sealed class DisposingImageSet(StorableImageSet inner) : StorableImageSet
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

	public IReadOnlyList<StorableImage> GetImagesRange(int index, int count)
	{
		return inner.GetImagesRange(index, count);
	}

	public StorableImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		return inner.CreateImage(creationTimestamp, size);
	}

	public void RemoveImageAt(int index)
	{
		var image = Images[index];
		image.Dispose();
		inner.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		for (int i = index; i < index + count; i++)
		{
			var image = Images[i];
			image.Dispose();
		}
		inner.RemoveImagesRange(index, count);
	}

	public void Dispose()
	{
		foreach (var image in Images)
			image.Dispose();
		inner.Dispose();
	}

	public StorableImage WrapAndInsertImage(StorableImage image)
	{
		return inner.WrapAndInsertImage(image);
	}
}