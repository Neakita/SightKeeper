using X11;

namespace SightKeeper.Application.Linux;

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