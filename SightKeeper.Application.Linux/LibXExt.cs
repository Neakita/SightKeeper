using System.Runtime.InteropServices;
using X11;

namespace SightKeeper.Application.Linux;

internal static class LibXExt
{
	private const string LIBEXT = "libXext";

	[DllImport(LIBEXT, EntryPoint = "XShmAttach",
		CallingConvention = CallingConvention.Cdecl)]
	public static extern int XShmAttach(IntPtr display, ref XShmSegmentInfo shminfo);
	// Bool XShmAttach( Display* /* dpy */, XShmSegmentInfo*	/* shminfo */ );


	// typedef struct _XDisplay Display
	// No, addresses aren't always positive - on x86_64,
	// pointers are sign-extended and the address space
	// is clustered symmetrically around 0
	// (though it is usual for the "negative" addresses to be kernel addresses).

	[DllImport(LIBEXT, EntryPoint = "XShmDetach",
		CallingConvention = CallingConvention.Cdecl)]
	public static extern unsafe bool XShmDetach(IntPtr display, XShmSegmentInfo* shminfo);


	// XShmCreateImage XShmGetImage XShmAttach XShmDetach 
	// nm /usr/lib/x86_64-linux-gnu/libXext.so.6 -D | grep "XShmCreateImage"

	[DllImport(/*LIBEXT*/ "libXext.so.6", EntryPoint = "XShmGetImage", CallingConvention = CallingConvention.Cdecl)]
	public static extern unsafe int XShmGetImage(IntPtr display, UIntPtr drawable, XImage* image, int x, int y, UIntPtr plane_mask);
	// Bool XShmGetImage(Display* /* dpy #1#, Drawable /* d #1#, XImage* /* image #1#, int /* x #1#, int /* y #1#, unsigned long /* plane_mask #1#);

	/*[System.Runtime.InteropServices.DllImport(LIBEXT, EntryPoint = "XShmCreateImage", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
	public static extern unsafe XImage* XShmCreateImage(System.IntPtr display, Visual* visual, uint depth
	     , int format, System.IntPtr data, ref XShmSegmentInfo shminfo, uint width, uint height);*/

	// XImage* ximg = XShmCreateImage(display, DefaultVisualOfScreen(screen), DefaultDepthOfScreen(screen)
	// , ZPixmap, NULL, &shminfo, window_attributes.width, window_attributes.height);
	// XImage *XShmCreateImage(Display* /* dpy */, Visual* /* visual */, unsigned int /* depth */,
	// int /* format */,char* /* data */, XShmSegmentInfo*	/* shminfo */, unsigned int	/* width */, unsigned int	/* height */ );


	/*public static unsafe Visual* DefaultVisualOfScreen(Screen* screen)
	{
	    return screen->root_visual;
	}

	public static unsafe int DefaultDepthOfScreen(Screen* screen)
	{
	    return screen->root_depth;
	}*/
}