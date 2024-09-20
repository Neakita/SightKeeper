using System.Runtime.InteropServices;

namespace SightKeeper.Application.Linux;
internal static class XLib
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
	public static extern unsafe ref XImage* XGetImage(
		IntPtr display,
		IntPtr drawable,
		int x,
		int y,
		uint width,
		uint height,
		ulong plane_mask,
		PixmapFormat format);

	[DllImport(libX11)]
	public static extern IntPtr XOpenDisplay(string? display);
	
	[DllImport(libX11)]
	public static extern int XDefaultScreen(IntPtr display);
	
	[DllImport(libX11)]
	public static extern int XCloseDisplay(IntPtr display);
	
	[DllImport(libX11)]
	public static extern IntPtr XDefaultVisual(IntPtr display, int screen_number);
}