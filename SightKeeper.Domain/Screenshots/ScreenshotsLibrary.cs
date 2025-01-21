namespace SightKeeper.Domain.Screenshots;

public sealed class ScreenshotsLibrary
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	/// <remarks>
	/// Sorted by date: first is the earliest, last is the latest
	/// </remarks>
	public IReadOnlyList<Screenshot> Screenshots => _screenshots;

	public Screenshot CreateScreenshot(DateTimeOffset creationDate, Vector2<ushort> imageSize)
	{
		if (_screenshots.Count > 0 && creationDate <= _screenshots[^1].CreationDate)
		{
			throw new InconsistentScreenshotCreationDateException(
				"An attempt was made to create a new screenshot earlier than the date of the last screenshot in the library. " +
				"Check that the time synchronization is correct and/or delete incorrectly created screenshots",
				creationDate, this);
		}
		Screenshot screenshot = new(creationDate, imageSize);
		_screenshots.Add(screenshot);
		return screenshot;
	}

	public void RemoveScreenshotAt(int index)
	{
		_screenshots.RemoveAt(index);
	}

	private readonly List<Screenshot> _screenshots = new();
}