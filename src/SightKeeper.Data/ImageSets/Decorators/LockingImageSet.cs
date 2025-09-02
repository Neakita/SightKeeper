using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;

namespace SightKeeper.Data.ImageSets.Decorators;

internal sealed class LockingImageSet(StorableImageSet inner, Lock editingLock) : StorableImageSet
{
	public string Name
	{
		get => inner.Name;
		set
		{
			lock (editingLock)
				inner.Name = value;
		}
	}

	public string Description
	{
		get => inner.Description;
		set
		{
			lock (editingLock)
				inner.Description = value;
		}
	}

	public IReadOnlyList<StorableImage> Images => inner.Images;

	public StorableImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		lock (editingLock)
			return inner.CreateImage(creationTimestamp, size);
	}

	public IReadOnlyList<StorableImage> GetImagesRange(int index, int count)
	{
		lock (editingLock)
			return inner.GetImagesRange(index, count);
	}

	public void RemoveImageAt(int index)
	{
		lock (editingLock)
			inner.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		lock (editingLock)
			inner.RemoveImagesRange(index, count);
	}

	public void Dispose()
	{
		inner.Dispose();
	}

	public StorableImage WrapAndInsertImage(StorableImage image)
	{
		return inner.WrapAndInsertImage(image);
	}
}