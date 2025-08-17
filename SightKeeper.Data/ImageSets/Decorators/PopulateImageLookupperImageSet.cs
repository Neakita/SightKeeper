using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;

namespace SightKeeper.Data.ImageSets.Decorators;

internal sealed class PopulateImageLookupperImageSet(StorableImageSet inner, ImageLookupperPopulator populator) : StorableImageSet
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

	public StorableImage WrapAndInsertImage(StorableImage image)
	{
		return inner.WrapAndInsertImage(image);
	}

	public StorableImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		var image = inner.CreateImage(creationTimestamp, size);
		populator.AddImage(image);
		return image;
	}

	public IReadOnlyList<StorableImage> GetImagesRange(int index, int count)
	{
		return inner.GetImagesRange(index, count);
	}

	public void RemoveImageAt(int index)
	{
		var image = Images[index];
		populator.RemoveImage(image);
		inner.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		for (int i = index; i < index + count; i++)
		{
			var image = Images[i];
			populator.RemoveImage(image);
		}
		inner.RemoveImagesRange(index, count);
	}

	public void Dispose()
	{
		foreach (var image in Images)
			populator.RemoveImage(image);
		inner.Dispose();
	}
}