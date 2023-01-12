using SightKeeper.Domain.Common;

namespace SightKeeper.Backend;

public interface IScreenshoter
{
	public ushort Width { get; set; }
	public ushort Height { get; set; }
	
	Image MakeScreenshot();
}