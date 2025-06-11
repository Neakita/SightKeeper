using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model;

internal sealed class LockingImageSet(ImageSet imageSet, Lock editingLock) : ImageSet, Decorator<ImageSet>
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

	public IReadOnlyList<Image> Images => imageSet.Images;

	public ImageSet Inner => imageSet;

	public Image CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		lock (editingLock)
			return imageSet.CreateImage(creationTimestamp, size);
	}

	public IReadOnlyList<Image> GetImagesRange(int index, int count)
	{
		lock (editingLock)
			return imageSet.GetImagesRange(index, count);
	}

	public void RemoveImage(Image image)
	{
		lock (editingLock)
			imageSet.RemoveImage(image);
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