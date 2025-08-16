using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Decorators;

internal sealed class StorableImageSetExtension(ImageSet inner, StorableImageSet extendedInner) : StorableImageSet
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

	public IReadOnlyList<StorableImage> Images => extendedInner.Images;

	public StorableImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		return (StorableImage)inner.CreateImage(creationTimestamp, size);
	}

	public IReadOnlyList<StorableImage> GetImagesRange(int index, int count)
	{
		return extendedInner.GetImagesRange(index, count);
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
		extendedInner.Dispose();
	}

	public void WrapAndInsertImage(StorableImage image)
	{
		extendedInner.WrapAndInsertImage(image);
	}
}