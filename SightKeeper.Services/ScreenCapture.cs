using System.Drawing;
using ScreenCapture.NET;

namespace SightKeeper.Services;

public sealed class ScreenCapture
{
	public void Do()
	{
		IEnumerable<GraphicsCard> graphicsCards = _captureService.GetGraphicsCards();
		IEnumerable<Display> displays = _captureService.GetDisplays(graphicsCards.First());
		IScreenCapture screenCapture = _captureService.GetScreenCapture(displays.First());
		CaptureZone fullscreen = screenCapture.RegisterCaptureZone(0, 0, screenCapture.Display.Width, screenCapture.Display.Height);
		screenCapture.CaptureScreen();
		lock (fullscreen.Buffer)
		{
			// Stride is the width in bytes of a row in the buffer (width in pixel * bytes per pixel)
			int stride = fullscreen.Stride;

			Span<byte> data = new(fullscreen.Buffer);

			// Iterate all rows of the image
			for (int y = 0; y < fullscreen.Height; y++)
			{
				// Select the actual data of the row
				Span<byte> row = data.Slice(y * stride, stride);

				// Iterate all pixels
				for (int x = 0; x < row.Length; x += fullscreen.BytesPerPixel)
				{
					// Data is in BGRA format for the DX11ScreenCapture
					byte b = row[x];
					byte g = row[x + 1];
					byte r = row[x + 2];
					byte a = row[x + 3];
				}
			}
		}

	}


	private readonly IScreenCaptureService _captureService = new DX11ScreenCaptureService();
}
