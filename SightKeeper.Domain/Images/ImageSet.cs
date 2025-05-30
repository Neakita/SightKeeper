namespace SightKeeper.Domain.Images;

public sealed class ImageSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	/// <remarks>
	/// Sorted by creation timestamp: first is the earliest, last is the latest
	/// </remarks>
	public IReadOnlyList<Image> Images => _images;

	public Image CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		if (_images.Count > 0 && creationTimestamp <= _images[^1].CreationTimestamp)
		{
			throw new InconsistentImageCreationTimestampException(
				"An attempt was made to create a new image earlier than the timestamp of the last image in the library. " +
				"Check that the time synchronization is correct and/or delete incorrectly created images",
				creationTimestamp, this);
		}
		Image image = new(this, creationTimestamp, size);
		_images.Add(image);
		return image;
	}

	public void RemoveImage(Image image)
	{
		ImageIsInUseException.ThrowForDeletionIfInUse(this, image);
		_images.Remove(image);
	}

	public void RemoveImageAt(int index)
	{
		var image = _images[index];
		ImageIsInUseException.ThrowForDeletionIfInUse(this, image);
		_images.RemoveAt(index);
	}

	public List<Image> GetImagesRange(int index, int count)
	{
		return _images.GetRange(index, count);
	}

	public void RemoveImagesRange(int index, int count)
	{
		for (var i = index; i < index + count; i++)
		{
			var image = _images[i];
			ImageIsInUseException.ThrowForDeletionIfInUse(this, image);
		}
		_images.RemoveRange(index, count);
	}

	private readonly List<Image> _images = new();
}