using Avalonia;

namespace SightKeeper.Application;

public interface ScreenBoundsProvider
{
	PixelSize MainScreenSize { get; }
	PixelPoint MainScreenCenter { get; }
}
