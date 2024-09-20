using X11;

namespace SightKeeper.Application.Linux;

// https://stackoverflow.com/questions/34176795/any-efficient-way-of-converting-ximage-data-to-pixel-map-e-g-array-of-rgb-quad
internal static class XLibShm
{
	private const int IPC_PRIVATE = 0; // or long?
	private const int BPP = 4; // Bytes per pixel?
	private const int IPC_CREAT = 01000; // wha?
	private const int IPC_RMID = 0;

	public static unsafe void createimage(IntPtr dsp, ShmImage* image, int width, int height)
	{
		// Create a shared memory area 
		image->shminfo.shmid = LibC.shmget(LibC.IPC_PRIVATE, width * height * BPP, LibC.IPC_CREAT | 0600);
		if (image->shminfo.shmid == -1)
		{
			throw new Exception();
		}

		// Map the shared memory segment into the address space of this process
		image->shminfo.shmaddr = (char*)LibC.shmat(image->shminfo.shmid, 0, 0);
		if (image->shminfo.shmaddr == (char*)-1)
		{
			throw new Exception();
		}

		image->data = (uint*)image->shminfo.shmaddr;
		image->shminfo.readOnly = false;

		// Mark the shared memory segment for removal
		// It will be removed even if this program crashes
		LibC.shmctl(image->shminfo.shmid, IPC_RMID, 0);

		// Allocate the memory needed for the XImage structure
		image->ximage = (XImage*)XShm.XShmCreateImage(dsp, Xlib.XDefaultVisual(dsp, Xlib.XDefaultScreen(dsp)),
			XLib.XDefaultDepth(dsp, Xlib.XDefaultScreen(dsp)), (int)PixmapFormat.ZPixmap, 0,
			&image->shminfo, 0, 0);
		if (image->ximage is null)
		{
			destroyimage(dsp, image);
			throw new Exception("could not allocate the XImage structure");
		}

		image->ximage->data = (IntPtr)image->data;
		image->ximage->width = width;
		image->ximage->height = height;

		// Ask the X server to attach the shared memory segment and sync
		XShm.XShmAttach(dsp, &image->shminfo);
		XLib.XSync(dsp, false);
	}

	public static unsafe void destroyimage(nint dsp, ShmImage* image)
	{
		if (image->ximage is null)
		{
			LibXExt.XShmDetach(dsp, &image->shminfo);
			XLib.XDestroyImage((IntPtr)image->ximage);
			image->ximage = null;
		}

		if (image->shminfo.shmaddr != (char*)-1)
		{
			LibC.shmdt((IntPtr)image->shminfo.shmaddr);
			image->shminfo.shmaddr = (char*)-1;
		}
	}
}