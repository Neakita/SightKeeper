using System.Runtime.InteropServices;
using SightKeeper.Application.Linux.X11.Natives;

namespace SightKeeper.Application.Linux.X11;

internal static class XLibShm
{
	public static unsafe void CreateImageSharedMemorySegment(IntPtr display, ShmImage* image, int width, int height)
	{
		CreateSharedMemorySegment(ref image->shminfo, width * height * sizeof(uint));
		image->data = MapSharedMemorySegment(ref image->shminfo);
		MarkSharedMemorySegmentForRemoval(image->shminfo);
		AllocateImageMemory(display, image, width, height);
		LibXExt.XShmAttach(display, &image->shminfo);
		LibX.XSync(display, false);
	}

	private static unsafe void AllocateImageMemory(IntPtr display, ShmImage* image, int width, int height)
	{
		var screen = LibX.XDefaultScreen(display);
		var visual = LibX.XDefaultVisual(display, screen);
		var depth = LibX.XDefaultDepth(display, screen);
		image->ximage = (XImage*)LibXExt.XShmCreateImage(
			display, visual, depth, (int)PixmapFormat.ZPixmap, 0, &image->shminfo, 0, 0);
		if (image->ximage is null)
		{
			DestroyImage(display, image);
			throw new Exception("could not allocate the XImage structure");
		}
		image->ximage->data = (IntPtr)image->data;
		image->ximage->width = width;
		image->ximage->height = height;
	}

	private static void MarkSharedMemorySegmentForRemoval(XShmSegmentInfo segmentInfo)
	{
		LibC.shmctl(segmentInfo.shmid, LibC.IPC_RMID, 0);
	}

	private static unsafe uint* MapSharedMemorySegment(ref XShmSegmentInfo segmentInfo)
	{
		segmentInfo.shmaddr = (char*)LibC.shmat(segmentInfo.shmid, 0, 0);
		if (segmentInfo.shmaddr == (char*)-1)
		{
			throw new Exception();
		}
		segmentInfo.readOnly = false;
		return (uint*)segmentInfo.shmaddr;
	}

	private static void CreateSharedMemorySegment(ref XShmSegmentInfo segmentInfo, nint size)
	{
		segmentInfo.shmid = LibC.shmget(LibC.IPC_PRIVATE, size, LibC.IPC_CREAT | 0600);
		if (segmentInfo.shmid == -1)
			throw new Exception(Marshal.GetLastPInvokeErrorMessage());
	}

	public static unsafe void DestroyImage(nint display, ShmImage* image)
	{
		if (image->ximage is not null)
		{
			LibXExt.XShmDetach(display, &image->shminfo);
			LibX.XDestroyImage((IntPtr)image->ximage);
			image->ximage = null;
		}

		if (image->shminfo.shmaddr != (char*)-1)
		{
			LibC.shmdt((IntPtr)image->shminfo.shmaddr);
			image->shminfo.shmaddr = (char*)-1;
		}
	}
}