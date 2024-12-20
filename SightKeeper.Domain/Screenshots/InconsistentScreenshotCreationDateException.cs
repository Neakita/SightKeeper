namespace SightKeeper.Domain.Screenshots;

public sealed class InconsistentScreenshotCreationDateException : Exception
{
	public DateTimeOffset NewScreenshotCreationDate { get; }
	public ScreenshotsLibrary AffectedLibrary { get; }

	public InconsistentScreenshotCreationDateException(string? message, DateTimeOffset newScreenshotCreationDate, ScreenshotsLibrary affectedLibrary) : base(message)
	{
		NewScreenshotCreationDate = newScreenshotCreationDate;
		AffectedLibrary = affectedLibrary;
	}
}