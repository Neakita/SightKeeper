#if OS_LINUX
using Autofac;
using SightKeeper.Application;
using SightKeeper.Application.Linux.X11;

namespace SightKeeper.Avalonia.Setup;

internal static class LinuxBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		builder.RegisterType<X11ScreenCapture>().As<ScreenCapture>();
	}
}
#endif