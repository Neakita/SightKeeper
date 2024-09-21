using SightKeeper.Application.Linux.X11.Natives;
using SightKeeper.Domain.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Linux.X11;

public sealed class X11ScreenCapture : ScreenCapture, IDisposable
{
	public X11ScreenCapture()
	{
		_display = LibX.XOpenDisplay(null);
		_screen = LibX.XDefaultScreen(_display);
		if (LibXExt.XShmQueryExtension(_display) == 0)
		{
			Dispose();
			throw new Exception("xserver doesn't support shm");
		}
	}

	public Image Capture(Vector2<ushort> resolution, Vector2<ushort> offset, Game? game)
	{
		if (_memorySegment?.Resolution != resolution)
		{
			_memorySegment?.Dispose();
			_memorySegment = new SharedImageMemorySegment<Bgra32>(_display, resolution);
		}
		_memorySegment.FetchData(_screen, offset);
		return Image.LoadPixelData(_memorySegment.Data, resolution.X, resolution.Y);
	}

	private readonly nint _display;
	private readonly int _screen;
	private SharedImageMemorySegment<Bgra32>? _memorySegment;

	private void ReleaseUnmanagedResources()
	{
		LibX.XCloseDisplay(_display);
	}

	~X11ScreenCapture()
	{
		Dispose(false);
	}

	private void Dispose(bool disposing)
	{
		ReleaseUnmanagedResources();
		if (disposing)
			_memorySegment?.Dispose();
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}