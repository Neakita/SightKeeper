using SightKeeper.Domain.Model;
using Image = SixLabors.ImageSharp.Image;

namespace SightKeeper.Application.Windows;

public sealed class WindowsScreenCapture : ScreenCapture
{
	public WindowsScreenCapture(ScreenBoundsProvider screenBoundsProvider)
	{
		_screenBoundsProvider = screenBoundsProvider;
	}
	
	public Image Capture(Vector2<ushort> resolution, Vector2<ushort> offset)
	{
		throw new NotImplementedException();
		/*var screenCenter = _screenBoundsProvider.MainScreenCenter;
		using Bitmap windowsBitmap = new(resolution.X, resolution.Y);
		using var graphics = Graphics.FromImage(windowsBitmap);
		var halfResolution = resolution / 2;
		Point position = new(screenCenter.X - halfResolution.X, screenCenter.Y - halfResolution.Y);
		Size size = new(resolution.X, resolution.Y);
		graphics.CopyFromScreen(position, Point.Empty, size);*/
	}

	private readonly ScreenBoundsProvider _screenBoundsProvider;
}
