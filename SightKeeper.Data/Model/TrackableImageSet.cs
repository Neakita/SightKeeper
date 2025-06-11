using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model;

internal sealed class TrackableImageSet(ImageSet imageSet, ChangeListener changeListener) : ImageSet, Decorator<ImageSet>
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

	public IReadOnlyList<Image> Images => imageSet.Images;

	public ImageSet Inner => imageSet;

	public Image CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		var image = imageSet.CreateImage(creationTimestamp, size);
		changeListener.SetDataChanged();
		return image;
	}

	public IReadOnlyList<Image> GetImagesRange(int index, int count)
	{
		return imageSet.GetImagesRange(index, count);
	}

	public void RemoveImage(Image image)
	{
		imageSet.RemoveImage(image);
		changeListener.SetDataChanged();
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