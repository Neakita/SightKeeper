namespace SightKeeper.Application;

public interface ScreenBoundsProvider
{
	int MainScreenWidth { get; }
	int MainScreenHeight { get; }
	int MainScreenHorizontalCenter => MainScreenWidth / 2;
	int MainScreenVerticalCenter => MainScreenHeight / 2;
}