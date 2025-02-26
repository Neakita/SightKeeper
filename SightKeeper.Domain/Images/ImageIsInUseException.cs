namespace SightKeeper.Domain.Images;

public sealed class ImageIsInUseException : Exception
{
	public static void ThrowForDeletionIfInUse(ImageSet set, Image image)
	{
		if (image.Assets.Count > 0)
			ThrowForDeletion(set, image);
	}

	private static void ThrowForDeletion(ImageSet set, Image image)
	{
		const string message =
			"The image is being used by some asset, so it cannot be deleted. " +
			"Delete the related assets before trying to delete the image";
		throw new ImageIsInUseException(message, set, image);
	}

	public ImageSet Set { get; }
	public Image Image { get; }

	public ImageIsInUseException(string? message, ImageSet set, Image image) : base(message)
	{
		Set = set;
		Image = image;
	}
}