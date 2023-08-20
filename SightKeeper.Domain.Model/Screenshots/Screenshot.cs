using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class Screenshot
{
	public ScreenshotImage Image { get; private set; }
	public DateTime CreationDate { get; private set; }
	public Asset? Asset { get; internal set; }
	
	internal Screenshot(byte[] content, Resolution resolution)
	{
		Image = new ScreenshotImage(this, content, resolution);
		CreationDate = DateTime.Now;
	}

	private Screenshot()
	{
		Image = null!;
	}
}