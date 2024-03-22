using SightKeeper.Domain;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public interface ScreenCapture
{
	byte[] Capture(ushort resolution, Game? game);
}