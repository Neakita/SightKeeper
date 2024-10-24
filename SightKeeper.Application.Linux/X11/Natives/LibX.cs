using System.Runtime.InteropServices;

namespace SightKeeper.Application.Linux.X11.Natives;
internal static unsafe partial class LibX
{
	private const string DllName = "libX11.so.6";

	[LibraryImport(DllName)]
	public static partial IntPtr XRootWindow(IntPtr display, int screen_number);

	[LibraryImport(DllName)]
	public static partial uint XDefaultDepth(IntPtr display, int screen_number);

	[LibraryImport(DllName)]
	public static partial int XDestroyImage(IntPtr image);

	[LibraryImport(DllName)]
	public static partial int XSync(IntPtr display, [MarshalAs(UnmanagedType.Bool)] bool discard);
	
	[LibraryImport(DllName)]
	public static partial XImage* XGetImage(IntPtr display, IntPtr drawable, int x, int y, uint width, uint height, ulong plane_mask, PixmapFormat format);

	[LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)] // not sure about StringMarshalling
	public static partial IntPtr XOpenDisplay(string? display);
	
	[LibraryImport(DllName)]
	public static partial int XDefaultScreen(IntPtr display);
	
	[LibraryImport(DllName)]
	public static partial int XCloseDisplay(IntPtr display);
	
	[LibraryImport(DllName)]
	public static partial IntPtr XDefaultVisual(IntPtr display, int screen_number);
	
	[DllImport(DllName, EntryPoint = "XDestroyImage")]
	public static extern int XDestroyImage2(XImage* image);
}