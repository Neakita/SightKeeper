// ReSharper disable InconsistentNaming
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
namespace SightKeeper.Application.Linux.X11;

internal struct XShmSegmentInfo
{
	public ulong shmseg;
	public int shmid;
	public unsafe char* shmaddr;
	public bool readOnly;
}