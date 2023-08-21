using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class Screenshot
{
	public ScreenshotsLibrary Library { get; private set; }
	public ScreenshotImage Image { get; private set; }
	public DateTime CreationDate { get; private set; }
	public Asset? Asset { get; internal set; }
	
	internal Screenshot(ScreenshotsLibrary library, byte[] content, Resolution resolution)
	{
		Library = library;
		Image = new ScreenshotImage(this, content, resolution);
		CreationDate = DateTime.Now;
	}

	private Screenshot()
	{
		Image = null!;
	}
}