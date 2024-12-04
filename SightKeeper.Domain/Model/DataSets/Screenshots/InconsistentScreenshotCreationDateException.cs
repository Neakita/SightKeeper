namespace SightKeeper.Domain.Model.DataSets.Screenshots;

public sealed class InconsistentScreenshotCreationDateException : Exception
{
	public DateTimeOffset NewScreenshotCreationDate { get; }
	public ScreenshotsLibrary AffectedLibrary { get; }

	public InconsistentScreenshotCreationDateException(DateTimeOffset newScreenshotCreationDate, ScreenshotsLibrary affectedLibrary)
	{
		NewScreenshotCreationDate = newScreenshotCreationDate;
		AffectedLibrary = affectedLibrary;
	}

	public InconsistentScreenshotCreationDateException(string? message, DateTimeOffset newScreenshotCreationDate, ScreenshotsLibrary affectedLibrary) : base(message)
	{
		NewScreenshotCreationDate = newScreenshotCreationDate;
		AffectedLibrary = affectedLibrary;
	}

	public InconsistentScreenshotCreationDateException(string? message, Exception? innerException, DateTimeOffset newScreenshotCreationDate, ScreenshotsLibrary affectedLibrary) : base(message, innerException)
	{
		NewScreenshotCreationDate = newScreenshotCreationDate;
		AffectedLibrary = affectedLibrary;
	}
}