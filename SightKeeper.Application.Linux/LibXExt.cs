using System.Runtime.InteropServices;

namespace SightKeeper.Application.Linux;

internal static class LibXExt
{
	private const string LIBEXT = "libXext.so.6";

	[DllImport(LIBEXT, EntryPoint = "XShmDetach",
		CallingConvention = CallingConvention.Cdecl)]
	public static extern unsafe bool XShmDetach(IntPtr display, XShmSegmentInfo* shminfo);

	[DllImport(LIBEXT, EntryPoint = "XShmGetImage", CallingConvention = CallingConvention.Cdecl)]
	public static extern unsafe int XShmGetImage(IntPtr display, UIntPtr drawable, XImage* image, int x, int y, UIntPtr plane_mask);
}