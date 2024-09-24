using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public interface ScreenBoundsProvider
{
	Vector2<ushort> MainScreenSize { get; }
	Vector2<ushort> MainScreenCenter => MainScreenSize / 2;
}