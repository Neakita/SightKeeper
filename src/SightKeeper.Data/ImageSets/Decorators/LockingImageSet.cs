using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Decorators;

internal sealed class LockingImageSet(ImageSet inner, Lock editingLock) : ImageSet
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

	public IReadOnlyList<ManagedImage> Images => inner.Images;

	public ManagedImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		lock (editingLock)
			return inner.CreateImage(creationTimestamp, size);
	}

	public IReadOnlyList<ManagedImage> GetImagesRange(int index, int count)
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
}