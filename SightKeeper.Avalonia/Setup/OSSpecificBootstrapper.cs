using Autofac;

namespace SightKeeper.Avalonia.Setup;

internal static class OSSpecificBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
#if (OS_WINDOWS)
		WindowsBootstrapper.Setup(builder);
#endif
	}
}