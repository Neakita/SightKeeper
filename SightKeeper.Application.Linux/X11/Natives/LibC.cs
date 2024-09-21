using System.Runtime.InteropServices;

namespace SightKeeper.Application.Linux.X11.Natives;

internal static partial class LibC
{
	public const int IPC_CREAT = 01000;
	public const int IPC_PRIVATE = 0;
	public const int IPC_RMID = 0;
	private const string DllName = "libc";

	[LibraryImport(DllName, SetLastError = true)]
	public static partial IntPtr shmat(int shmid, IntPtr shmaddr, int shmflg);

	[LibraryImport(DllName, SetLastError = true)]
	public static partial int shmget(int key, IntPtr size, int shmflg);

	[LibraryImport(DllName)]
	public static partial int shmctl(int shmid, int cmd, int buf);

	[LibraryImport(DllName, SetLastError = true)]
	public static partial int shmdt(IntPtr shmaddr);
}