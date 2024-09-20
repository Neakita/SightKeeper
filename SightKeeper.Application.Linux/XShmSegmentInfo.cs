namespace SightKeeper.Application.Linux;

internal struct XShmSegmentInfo
{
	public ulong shmseg;
	public int shmid;
	public unsafe char* shmaddr;
	public bool readOnly;
}