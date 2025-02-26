namespace SightKeeper.Domain.Images;

public sealed class InconsistentScreenshotCreationTimestampException : Exception
{
	public DateTimeOffset NewScreenshotCreationTimestamp { get; }
	public ImageSet AffectedLibrary { get; }

	public InconsistentScreenshotCreationTimestampException(string? message, DateTimeOffset newScreenshotCreationTimestamp, ImageSet affectedLibrary) : base(message)
	{
		NewScreenshotCreationTimestamp = newScreenshotCreationTimestamp;
		AffectedLibrary = affectedLibrary;
	}
}