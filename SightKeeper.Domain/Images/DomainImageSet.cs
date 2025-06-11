namespace SightKeeper.Domain.Images;

public sealed class DomainImageSet(ImageSet imageSet) : ImageSet, Decorator<ImageSet>
{
	public string Name
	{
		get => imageSet.Name;
		set => imageSet.Name = value;
	}

	public string Description
	{
		get => imageSet.Description;
		set => imageSet.Description = value;
	}

	/// <remarks>
	/// Sorted by creation timestamp: first is the earliest, last is the latest
	/// </remarks>
	public IReadOnlyList<Image> Images => imageSet.Images;

	public ImageSet Inner => imageSet;

	public Image CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		if (Images.Count > 0 && creationTimestamp <= Images[^1].CreationTimestamp)
		{
			throw new InconsistentImageCreationTimestampException(
				"An attempt was made to create a new image earlier than the timestamp of the last image in the library. " +
				"Check that the time synchronization is correct and/or delete incorrectly created images",
				creationTimestamp, this);
		}
		return imageSet.CreateImage(creationTimestamp, size);
	}

	public IReadOnlyList<Image> GetImagesRange(int index, int count)
	{
		return imageSet.GetImagesRange(index, count);
	}

	public void RemoveImage(Image image)
	{
		ImageIsInUseException.ThrowForDeletionIfInUse(this, image);
		imageSet.RemoveImage(image);
	}

	public void RemoveImageAt(int index)
	{
		var image = Images[index];
		ImageIsInUseException.ThrowForDeletionIfInUse(this, image);
		imageSet.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		for (var i = index; i < index + count; i++)
		{
			var image = Images[i];
			ImageIsInUseException.ThrowForDeletionIfInUse(this, image);
		}
		imageSet.RemoveImagesRange(index, count);
	}
}