namespace SightKeeper.Domain.Images;

public sealed class ScreenshotIsInUseException : Exception
{
	public static void ThrowForDeletionIfInUse(ImageSet library, Image image)
	{
		if (image.Assets.Count > 0)
			ThrowForDeletion(library, image);
	}

	private static void ThrowForDeletion(ImageSet library, Image image)
	{
		const string message =
			"The screenshot is being used by some asset, so it cannot be deleted. " +
			"Delete the related assets before trying to delete the screenshot";
		throw new ScreenshotIsInUseException(message, library, image);
	}

	public ImageSet Library { get; }
	public Image Image { get; }

	public ScreenshotIsInUseException(string? message, ImageSet library, Image image) : base(message)
	{
		Library = library;
		Image = image;
	}
}