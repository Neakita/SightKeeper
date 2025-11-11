using SightKeeper.Domain;

namespace SightKeeper.Application.Interop;

public interface ScreenBoundsProvider
{
	Vector2<ushort> MainScreenSize { get; }
	Vector2<ushort> MainScreenCenter => MainScreenSize / 2;
}