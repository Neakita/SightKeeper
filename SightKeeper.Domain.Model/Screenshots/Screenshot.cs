using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class Screenshot
{
	public Image Image { get; private set; }
	public DateTime CreationDate { get; private set; }
	public Asset? Asset { get; internal set; }
	public ScreenshotsLibrary Library { get; private set; }

	internal Screenshot(byte[] content, ScreenshotsLibrary library)
	{
		Library = library;
		Image = new Image(content);
		CreationDate = DateTime.Now;
	}

	private Screenshot()
	{
		Image = null!;
		Library = null!;
	}
}