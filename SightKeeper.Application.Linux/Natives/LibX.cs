using System.Runtime.InteropServices;

namespace SightKeeper.Application.Linux.Natives;
internal static partial class LibX
{
	private const string libX11 = "libX11.so.6";

	[DllImport(libX11)]
	public static extern IntPtr XRootWindow(IntPtr display, int screen_number);

	[DllImport(libX11)]
	public static extern uint XDefaultDepth(IntPtr display, int screen_number);

	[DllImport(libX11)]
	public static extern int XDestroyImage(IntPtr image);

	[DllImport(libX11)]
	public static extern int XSync(IntPtr display, bool discard);
	
	[DllImport(libX11)]
	public static extern unsafe ref XImage* XGetImage(IntPtr display, IntPtr drawable, int x, int y, uint width, uint height, ulong plane_mask, PixmapFormat format);

	[LibraryImport(libX11, StringMarshalling = StringMarshalling.Utf8)] // not sure about StringMarshalling
	public static partial IntPtr XOpenDisplay(string? display);
	
	[LibraryImport(libX11)]
	public static partial int XDefaultScreen(IntPtr display);
	
	[LibraryImport(libX11)]
	public static partial int XCloseDisplay(IntPtr display);
	
	[LibraryImport(libX11)]
	public static partial IntPtr XDefaultVisual(IntPtr display, int screen_number);
}