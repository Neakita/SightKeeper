using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal sealed class DeletableDataImageSet(ImageSet inner) : ImageSet, DeletableData, Decorator<ImageSet>
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

	public void DeleteData()
	{
		foreach (var image in Images)
			image.DeleteData();
	}

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
		inner.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		inner.RemoveImagesRange(index, count);
	}

	public void Dispose()
	{
		inner.Dispose();
	}
}