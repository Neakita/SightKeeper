using SightKeeper.Domain;

namespace SightKeeper.Application.Tests.Capturing;

internal sealed class FakeScreenBoundsProvider : ScreenBoundsProvider
{
	public Vector2<ushort> MainScreenSize { get; }

	public FakeScreenBoundsProvider(Vector2<ushort> mainScreenSize)
	{
		MainScreenSize = mainScreenSize;
	}
}