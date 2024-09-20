using CommunityToolkit.Diagnostics;
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
		_display = XLib.XOpenDisplay(null);
		_screen = XLib.XDefaultScreen(_display);
		_window = XLib.XRootWindow(_display, _screen);
		if (XShm.XShmQueryExtension(_display) == 0)
		{
			XLib.XCloseDisplay(_display);
			throw new Exception("xserver doesn't support shm");
		}
	}

	public unsafe Stream Capture(Vector2<ushort> resolution, Game? game)
	{
		int AllPlanes = ~0;
		UIntPtr AllPlanes2 = new UIntPtr((uint)AllPlanes);
		MemoryStream stream = new();
		ShmImage image = new();
		XLibShm.createimage(_display, &image, resolution.X, resolution.Y);;
		LibXExt.XShmGetImage(_display, (UIntPtr)XLib.XRootWindow(_display, _screen), image.ximage, 0, 0, AllPlanes2);
		ReadOnlySpan<Bgra32> data = new(image.data, resolution.X * resolution.Y);
		var array = data.ToArray();
		var sum = array.Sum(x => x.PackedValue);
		Guard.IsGreaterThan(sum, 0);
		Image.LoadPixelData(data, resolution.X, resolution.Y).Save(stream, Encoder);
		XLibShm.destroyimage(_display, &image);
		stream.Position = 0;
		return stream;
	}

	public void Dispose()
	{
		ReleaseUnmanagedResources();
		GC.SuppressFinalize(this);
	}

	private readonly nint _display;
	private readonly int _screen;
	private readonly IntPtr _window;

	private unsafe XImage* GetXImage(Vector2<ushort> resolution, Vector2<ushort> offset)
	{
		return XLib.XGetImage(
			_display,
			_window,
			offset.X,
			offset.Y,
			resolution.X,
			resolution.Y,
			(ulong)Planes.AllPlanes,
			PixmapFormat.ZPixmap);
	}

	private void ReleaseUnmanagedResources()
	{
		XLib.XCloseDisplay(_display);
	}

	~X11ScreenCapture()
	{
		ReleaseUnmanagedResources();
	}
}