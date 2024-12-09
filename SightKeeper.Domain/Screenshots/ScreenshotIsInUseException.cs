namespace SightKeeper.Domain.Screenshots;

public sealed class ScreenshotIsInUseException : Exception
{
	public static void ThrowForDeletionIfInUse(ScreenshotsLibrary library, Screenshot screenshot)
	{
		if (screenshot.Assets.Count > 0)
			ThrowForDeletion(library, screenshot);
	}

	private static void ThrowForDeletion(ScreenshotsLibrary library, Screenshot screenshot)
	{
		const string message =
			"The screenshot is being used by some asset, so it cannot be deleted. " +
			"Delete the related assets before trying to delete the screenshot";
		throw new ScreenshotIsInUseException(message, library, screenshot);
	}

	public ScreenshotsLibrary Library { get; }
	public Screenshot Screenshot { get; }

	public ScreenshotIsInUseException(string? message, ScreenshotsLibrary library, Screenshot screenshot) : base(message)
	{
		Library = library;
		Screenshot = screenshot;
	}
}