using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class Screenshot
{
	public byte[] Content { get; private set; }
	public DateTime CreationDate { get; private set; }
	public ScreenshotsLibrary? Library { get; internal set; }
	internal Asset? Asset { get; set; }
	
	internal Screenshot(ScreenshotsLibrary library, byte[] content)
	{
		Library = library;
		Content = content;
		CreationDate = DateTime.UtcNow;
	}

	private Screenshot()
	{
		Library = null!;
		Content = null!;
	}
}