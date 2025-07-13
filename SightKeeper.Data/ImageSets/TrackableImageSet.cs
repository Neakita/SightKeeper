using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;

namespace SightKeeper.Data.ImageSets;

internal sealed class TrackableImageSet(StorableImageSet imageSet, ChangeListener changeListener) : StorableImageSet
{
	public string Name
	{
		get => imageSet.Name;
		set
		{
			imageSet.Name = value;
			changeListener.SetDataChanged();
		}
	}

	public string Description
	{
		get => imageSet.Description;
		set
		{
			imageSet.Description = value;
			changeListener.SetDataChanged();
		}
	}

	public IReadOnlyList<StorableImage> Images => imageSet.Images;

	public StorableImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		var image = imageSet.CreateImage(creationTimestamp, size);
		changeListener.SetDataChanged();
		return image;
	}

	public IReadOnlyList<StorableImage> GetImagesRange(int index, int count)
	{
		return imageSet.GetImagesRange(index, count);
	}

	public void RemoveImageAt(int index)
	{
		imageSet.RemoveImageAt(index);
		changeListener.SetDataChanged();
	}

	public void RemoveImagesRange(int index, int count)
	{
		if (count == 0)
			return;
		imageSet.RemoveImagesRange(index, count);
		changeListener.SetDataChanged();
	}
}