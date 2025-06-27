using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal sealed class DataRemovingImageSet(ImageSet inner) : ImageSet
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

	public IReadOnlyList<Image> Images => inner.Images;

	public Image CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		return inner.CreateImage(creationTimestamp, size);
	}

	public IReadOnlyList<Image> GetImagesRange(int index, int count)
	{
		return inner.GetImagesRange(index, count);
	}

	public void RemoveImageAt(int index)
	{
		var image = Images[index].UnWrapDecorator<StreamableDataImage>();
		image.DeleteData();
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
		var image = Images[index].UnWrapDecorator<StreamableDataImage>();
		image.DeleteData();
	}
}