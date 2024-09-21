using System.Runtime.InteropServices;

namespace SightKeeper.Application.Linux.X11.Natives;
internal static unsafe partial class LibX
{
	private const string libX11 = "libX11.so.6";

	[LibraryImport(libX11)]
	public static partial IntPtr XRootWindow(IntPtr display, int screen_number);

	[LibraryImport(libX11)]
	public static partial uint XDefaultDepth(IntPtr display, int screen_number);

	[LibraryImport(libX11)]
	public static partial int XDestroyImage(IntPtr image);

	[LibraryImport(libX11)]
	public static partial int XSync(IntPtr display, [MarshalAs(UnmanagedType.Bool)] bool discard);
	
	[LibraryImport(libX11)]
	public static partial XImage* XGetImage(IntPtr display, IntPtr drawable, int x, int y, uint width, uint height, ulong plane_mask, PixmapFormat format);

	[LibraryImport(libX11, StringMarshalling = StringMarshalling.Utf8)] // not sure about StringMarshalling
	public static partial IntPtr XOpenDisplay(string? display);
	
	[LibraryImport(libX11)]
	public static partial int XDefaultScreen(IntPtr display);
	
	[LibraryImport(libX11)]
	public static partial int XCloseDisplay(IntPtr display);
	
	[LibraryImport(libX11)]
	public static partial IntPtr XDefaultVisual(IntPtr display, int screen_number);
}