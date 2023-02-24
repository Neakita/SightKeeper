using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application;

public interface IScreenshoter
{
	public ushort Width { get; set; }
	public ushort Height { get; set; }
	
	Image MakeScreenshot();
}