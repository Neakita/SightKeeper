using SightKeeper.Application.Linux.Natives;
using SightKeeper.Domain.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Linux;

public sealed class X11ScreenCapture : ScreenCapture, IDisposable
{
	private static readonly IImageEncoder Encoder = new BmpEncoder();

	public X11ScreenCapture()
	{
		_display = LibX.XOpenDisplay(null);
		_screen = LibX.XDefaultScreen(_display);
		if (LibXExt.XShmQueryExtension(_display) == 0)
		{
			LibX.XCloseDisplay(_display);
			throw new Exception("xserver doesn't support shm");
		}
	}

	public unsafe Stream Capture(Vector2<ushort> resolution, Game? game)
	{
		if (_memorySegment?.Resolution != resolution)
		{
			_memorySegment?.Dispose();
			_memorySegment = new SharedImageMemorySegment<Bgra32>(_display, resolution);
		}
		_memorySegment.FetchData(_screen, new Vector2<ushort>());
		MemoryStream stream = new();
		ShmImage image = new();
		Image.LoadPixelData(_memorySegment.Data, resolution.X, resolution.Y).Save(stream, Encoder);
		XLibShm.DestroyImage(_display, &image);
		stream.Position = 0;
		return stream;
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