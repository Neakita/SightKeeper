using SharpHook.Providers;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public sealed class SharpHookScreenBoundsProvider : ScreenBoundsProvider
{
	public Vector2<ushort> MainScreenSize => GetPrimaryScreenBounds();

	private static Vector2<ushort> GetPrimaryScreenBounds()
	{
		var screenData = UioHookProvider.Instance.CreateScreenInfo().Single();
		return new Vector2<ushort>(screenData.Width, screenData.Height);
	}
}