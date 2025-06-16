using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

internal sealed class TrackableImageSet(ImageSet imageSet, ChangeListener changeListener) : ImageSet
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

	public IReadOnlyList<DomainImage> Images => imageSet.Images;

	public ImageSet Inner => imageSet;

	public DomainImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		var image = imageSet.CreateImage(creationTimestamp, size);
		changeListener.SetDataChanged();
		return image;
	}

	public IReadOnlyList<DomainImage> GetImagesRange(int index, int count)
	{
		return imageSet.GetImagesRange(index, count);
	}

	public int IndexOf(DomainImage image)
	{
		return imageSet.IndexOf(image);
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