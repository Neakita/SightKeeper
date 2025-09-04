using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Decorators;

internal sealed class TrackableImageSet(ImageSet inner, ChangeListener changeListener) : ImageSet
{
	public string Name
	{
		get => inner.Name;
		set
		{
			inner.Name = value;
			changeListener.SetDataChanged();
		}
	}

	public string Description
	{
		get => inner.Description;
		set
		{
			inner.Description = value;
			changeListener.SetDataChanged();
		}
	}

	public IReadOnlyList<ManagedImage> Images => inner.Images;

	public ManagedImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		var image = inner.CreateImage(creationTimestamp, size);
		changeListener.SetDataChanged();
		return image;
	}

	public IReadOnlyList<ManagedImage> GetImagesRange(int index, int count)
	{
		return inner.GetImagesRange(index, count);
	}

	public void RemoveImageAt(int index)
	{
		inner.RemoveImageAt(index);
		changeListener.SetDataChanged();
	}

	public void RemoveImagesRange(int index, int count)
	{
		if (count == 0)
			return;
		inner.RemoveImagesRange(index, count);
		changeListener.SetDataChanged();
	}

	public void Dispose()
	{
		inner.Dispose();
	}
}