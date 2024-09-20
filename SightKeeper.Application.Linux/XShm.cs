using System.Runtime.InteropServices;
using X11;

namespace SightKeeper.Application.Linux;


internal static unsafe class XShm
{
	[DllImport("libXext.so.6", SetLastError = true)]
	public static extern int XShmQueryExtension(IntPtr display);

	/*
	Status XShmQueryVersion (display, major, minor, pixmaps)
	  Display *display;
	  int *major, *minor;
	  Bool *pixmaps
	*/
	[DllImport("libXext.so.6", SetLastError = true)]
	public static extern int XShmQueryVersion(IntPtr display, out int major, out int minor, out bool pixmaps);

	[DllImport("libXext.so.6", SetLastError = true)]
	public static extern int XShmPutImage(IntPtr display, IntPtr drawable, IntPtr gc, XImage* image, int src_x, int src_y,
		int dst_x, int dst_y, uint src_width, uint src_height, bool send_event);

	// XShmAttach(display, &shminfo);
	[DllImport("libXext.so.6", SetLastError = true)]
	public static extern int XShmAttach(IntPtr display, XShmSegmentInfo* shminfo);

	/*
	XImage *XShmCreateImage (display, visual, depth, format, data,
	                   shminfo, width, height)
	  Display *display;
	  Visual *visual;
	  unsigned int depth, width, height;
	  int format;
	  char *data;
	  XShmSegmentInfo *shminfo;
	*/
	[DllImport("libXext.so.6", SetLastError = true)]
	public static extern IntPtr XShmCreateImage(IntPtr display, IntPtr visual, uint depth, int format, IntPtr data,
		XShmSegmentInfo* shminfo, uint width, uint height);
}