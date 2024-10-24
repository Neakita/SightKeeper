using CommunityToolkit.HighPerformance;
using SightKeeper.Application.Linux.X11.Natives;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Domain.Model;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Linux.X11;

public sealed class X11ScreenCapture : ScreenCapture<Bgra32>, IDisposable
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

	public ReadOnlySpan2D<Bgra32> Capture(Vector2<ushort> resolution, Vector2<ushort> offset)
	{
		if (_memorySegment?.Resolution != resolution)
		{
			_memorySegment?.Dispose();
			_memorySegment = new SharedImageMemorySegment<Bgra32>(_display, resolution);
		}
		_memorySegment.FetchData(_screen, offset);
		return _memorySegment.Data;
	}

	public void Dispose()
	{
		_memorySegment?.Dispose();
		ReleaseUnmanagedResources();
		GC.SuppressFinalize(this);
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
		Dispose();
	}
}