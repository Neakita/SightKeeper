using Autofac;

namespace SightKeeper.Avalonia.Setup;

internal static class OSSpecificBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
#if (OS_WINDOWS)
		WindowsBootstrapper.Setup(builder);
#elif (OS_LINUX)
		LinuxBootstrapper.Setup(builder);
#endif
	}
}