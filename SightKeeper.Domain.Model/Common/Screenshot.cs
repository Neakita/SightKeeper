namespace SightKeeper.Domain.Model.Common;

public sealed class Screenshot
{
	public Image Image { get; private set; }
	public DateTime CreationDate { get; private set; }
	internal ScreenshotsLibrary? Library { get; set; }
	
	internal Screenshot(ScreenshotsLibrary library, Image image)
	{
		Library = library;
		Image = image;
		CreationDate = DateTime.UtcNow;
	}

	private Screenshot()
	{
		Library = null!;
		Image = null!;
	}
}