using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

internal sealed class LockingImageSet(ImageSet imageSet, Lock editingLock) : ImageSet
{
	public string Name
	{
		get => imageSet.Name;
		set
		{
			lock (editingLock)
				imageSet.Name = value;
		}
	}

	public string Description
	{
		get => imageSet.Description;
		set
		{
			lock (editingLock)
				imageSet.Description = value;
		}
	}

	public IReadOnlyList<DomainImage> Images => imageSet.Images;

	public ImageSet Inner => imageSet;

	public DomainImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		lock (editingLock)
			return imageSet.CreateImage(creationTimestamp, size);
	}

	public IReadOnlyList<DomainImage> GetImagesRange(int index, int count)
	{
		lock (editingLock)
			return imageSet.GetImagesRange(index, count);
	}

	public int IndexOf(DomainImage image)
	{
		return imageSet.IndexOf(image);
	}

	public void RemoveImageAt(int index)
	{
		lock (editingLock)
			imageSet.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		lock (editingLock)
			imageSet.RemoveImagesRange(index, count);
	}
}