using System.Runtime.InteropServices;

namespace SightKeeper.Application.Linux.Natives;

internal static unsafe partial class LibXExt
{
	private const string DllName = "libXext.so.6";

	[LibraryImport(DllName)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool XShmDetach(IntPtr display, XShmSegmentInfo* shminfo);

	[LibraryImport(DllName)]
	public static partial int XShmGetImage(IntPtr display, UIntPtr drawable, XImage* image, int x, int y, UIntPtr plane_mask);

	[LibraryImport(DllName, SetLastError = true)]
	public static partial int XShmQueryExtension(IntPtr display);

	[LibraryImport(DllName, SetLastError = true)]
	public static partial int XShmAttach(IntPtr display, XShmSegmentInfo* shminfo);

	[LibraryImport(DllName, SetLastError = true)]
	public static partial IntPtr XShmCreateImage(IntPtr display, IntPtr visual, uint depth, int format, IntPtr data, XShmSegmentInfo* shminfo, uint width, uint height);
}