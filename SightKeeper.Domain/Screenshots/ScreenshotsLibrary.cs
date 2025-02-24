namespace SightKeeper.Domain.Screenshots;

public sealed class ScreenshotsLibrary
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	/// <remarks>
	/// Sorted by creation timestamp: first is the earliest, last is the latest
	/// </remarks>
	public IReadOnlyList<Screenshot> Screenshots => _screenshots;

	public Screenshot CreateScreenshot(DateTimeOffset creationTimestamp, Vector2<ushort> imageSize)
	{
		if (_screenshots.Count > 0 && creationTimestamp <= _screenshots[^1].CreationTimestamp)
		{
			throw new InconsistentScreenshotCreationTimestampException(
				"An attempt was made to create a new screenshot earlier than the timestamp of the last screenshot in the library. " +
				"Check that the time synchronization is correct and/or delete incorrectly created screenshots",
				creationTimestamp, this);
		}
		Screenshot screenshot = new(creationTimestamp, imageSize);
		_screenshots.Add(screenshot);
		return screenshot;
	}

	public void RemoveScreenshotAt(int index)
	{
		var screenshot = _screenshots[index];
		ScreenshotIsInUseException.ThrowForDeletionIfInUse(this, screenshot);
		_screenshots.RemoveAt(index);
	}

	private readonly List<Screenshot> _screenshots = new();
}