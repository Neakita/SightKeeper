namespace SightKeeper.Domain.Screenshots;

public sealed class InconsistentScreenshotCreationTimestampException : Exception
{
	public DateTimeOffset NewScreenshotCreationTimestamp { get; }
	public ScreenshotsLibrary AffectedLibrary { get; }

	public InconsistentScreenshotCreationTimestampException(string? message, DateTimeOffset newScreenshotCreationTimestamp, ScreenshotsLibrary affectedLibrary) : base(message)
	{
		NewScreenshotCreationTimestamp = newScreenshotCreationTimestamp;
		AffectedLibrary = affectedLibrary;
	}
}