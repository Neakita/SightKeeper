using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public interface ScreenBoundsProvider
{
	Vector2<int> MainScreenSize { get; }
	Vector2<int> MainScreenCenter => MainScreenSize / 2;
}