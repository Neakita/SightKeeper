using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

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

	public IReadOnlyList<Image> Images => inner.Images;

	public Image CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		lock (editingLock)
			return inner.CreateImage(creationTimestamp, size);
	}

	public IReadOnlyList<Image> GetImagesRange(int index, int count)
	{
		lock (editingLock)
			return inner.GetImagesRange(index, count);
	}

	public int IndexOf(Image image)
	{
		return inner.IndexOf(image);
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
}