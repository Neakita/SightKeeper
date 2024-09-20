using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public interface ScreenCapture
{
	Stream Capture(Vector2<ushort> resolution, Game? game);
}