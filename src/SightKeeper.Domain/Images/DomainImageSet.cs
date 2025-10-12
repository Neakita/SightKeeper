namespace SightKeeper.Domain.Images;

public sealed class DomainImageSet(ImageSet inner) : ImageSet, Decorator<ImageSet>
{
	public string Name
	{
		get => inner.Name;
		set => inner.Name = value;
	}

	public string Description
	{
		get => inner.Description;
		set => inner.Description = value;
	}

	/// <remarks>
	/// Sorted by creation timestamp: first is the earliest, last is the latest
	/// </remarks>
	public IReadOnlyList<ManagedImage> Images => inner.Images;
	public ImageSet Inner => inner;

	public ManagedImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		ValidateImageSize(size);
		if (Images.Count > 0 && creationTimestamp <= Images[^1].CreationTimestamp)
		{
			throw new InconsistentImageCreationTimestampException(
				"An attempt was made to create a new image earlier than the timestamp of the last image in the library. " +
				"Check that the time synchronization is correct and/or delete incorrectly created images",
				creationTimestamp, this);
		}
		return inner.CreateImage(creationTimestamp, size);
	}

	public IReadOnlyList<ManagedImage> GetImagesRange(int index, int count)
	{
		return inner.GetImagesRange(index, count);
	}

	public void RemoveImageAt(int index)
	{
		var image = Images[index];
		ImageIsInUseException.ThrowForDeletionIfInUse(this, image);
		inner.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		for (var i = index; i < index + count; i++)
		{
			var image = Images[i];
			ImageIsInUseException.ThrowForDeletionIfInUse(this, image);
		}
		inner.RemoveImagesRange(index, count);
	}

	public void Dispose()
	{
		inner.Dispose();
	}

	private static void ValidateImageSize(Vector2<ushort> size)
	{
		ValidateImageSize(size.X);
		ValidateImageSize(size.Y);
	}

	private static void ValidateImageSize(ushort size)
	{
		if (size <= 0)
			throw new ArgumentException($"Image size should be greater than 0, but was {size}");
	}
}