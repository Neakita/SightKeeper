namespace SightKeeper.Application.Linux.X11;

internal struct XShmSegmentInfo
{
	public ulong shmseg;
	public int shmid;
	public unsafe char* shmaddr;
	public bool readOnly;
}