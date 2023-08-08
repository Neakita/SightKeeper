using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class Screenshot
{
	public ScreenshotImage Image { get; private set; }
	public DateTime CreationDate { get; private set; }
	public ScreenshotsLibrary Library { get; internal set; }
	public Asset? Asset { get; internal set; }
	
	internal Screenshot(ScreenshotsLibrary library, byte[] content)
	{
		Library = library;
		Image = new ScreenshotImage(this, content);
		CreationDate = DateTime.Now;
	}

	private Screenshot()
	{
		Library = null!;
		Image = null!;
	}
}