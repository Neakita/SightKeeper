namespace SightKeeper.Domain.Images;

public sealed class ImageSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	/// <remarks>
	/// Sorted by creation timestamp: first is the earliest, last is the latest
	/// </remarks>
	public IReadOnlyList<Image> Screenshots => _screenshots;

	public Image CreateScreenshot(DateTimeOffset creationTimestamp, Vector2<ushort> imageSize)
	{
		if (_screenshots.Count > 0 && creationTimestamp <= _screenshots[^1].CreationTimestamp)
		{
			throw new InconsistentScreenshotCreationTimestampException(
				"An attempt was made to create a new screenshot earlier than the timestamp of the last screenshot in the library. " +
				"Check that the time synchronization is correct and/or delete incorrectly created screenshots",
				creationTimestamp, this);
		}
		Image image = new(creationTimestamp, imageSize);
		_screenshots.Add(image);
		return image;
	}

	public void RemoveScreenshotAt(int index)
	{
		var screenshot = _screenshots[index];
		ScreenshotIsInUseException.ThrowForDeletionIfInUse(this, screenshot);
		_screenshots.RemoveAt(index);
	}

	private readonly List<Image> _screenshots = new();
}