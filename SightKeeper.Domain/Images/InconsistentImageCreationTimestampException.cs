namespace SightKeeper.Domain.Images;

public sealed class InconsistentImageCreationTimestampException : Exception
{
	public DateTimeOffset NewScreenshotCreationTimestamp { get; }
	public ImageSet AffectedLibrary { get; }

	public InconsistentImageCreationTimestampException(string? message, DateTimeOffset newScreenshotCreationTimestamp, ImageSet affectedLibrary) : base(message)
	{
		NewScreenshotCreationTimestamp = newScreenshotCreationTimestamp;
		AffectedLibrary = affectedLibrary;
	}
}