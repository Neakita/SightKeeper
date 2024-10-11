using System.Drawing;
using System.Drawing.Imaging;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Model;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Windows;

public sealed class WindowsScreenCapture : ScreenCapture
{
	public unsafe ReadOnlySpan2D<Bgra32> Capture(Vector2<ushort> resolution, Vector2<ushort> offset)
	{
		if (_bitmap != null)
		{
			Guard.IsNotNull(_bitmapData);
			_bitmap.UnlockBits(_bitmapData);
			if (_bitmap.Width != resolution.X || _bitmap.Height != resolution.Y)
			{
				_bitmap.Dispose();
				_bitmap = null;
			}
		}
		if (_bitmap == null)
		{
			_bitmap = new Bitmap(resolution.X, resolution.Y);
			_graphics = Graphics.FromImage(_bitmap);
		}
		Point position = new(offset.X, offset.Y);
		Size size = new(resolution.X, resolution.Y);
		Guard.IsNotNull(_graphics);
		_graphics.CopyFromScreen(position, Point.Empty, size);
		_bitmapData = _bitmap.LockBits(new Rectangle(Point.Empty, size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
		return new ReadOnlySpan2D<Bgra32>((void*)_bitmapData.Scan0, _bitmapData.Height, _bitmapData.Width, 0);
	}

	private Bitmap? _bitmap;
	private BitmapData? _bitmapData;
	private Graphics? _graphics;
}
