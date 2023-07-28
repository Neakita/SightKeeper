using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model;

public sealed class Screenshot
{
	public Image Image { get; private set; }
	public DateTime CreationDate { get; private set; }
	public ScreenshotsLibrary? Library { get; internal set; }
	internal DetectorAsset? DetectorAsset { get; set; }
	
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