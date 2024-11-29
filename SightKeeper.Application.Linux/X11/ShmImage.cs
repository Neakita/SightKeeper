// ReSharper disable InconsistentNaming
namespace SightKeeper.Application.Linux.X11;

internal struct ShmImage
{
	public XShmSegmentInfo shminfo;
	public unsafe XImage* ximage;
	public unsafe uint* data;

	public unsafe ShmImage()
	{
		ximage = null;
		shminfo.shmaddr = (char*)-1;
	}
}