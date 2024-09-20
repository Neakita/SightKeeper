using Autofac;
using SightKeeper.Application;
using SightKeeper.Application.Linux;

namespace SightKeeper.Avalonia.Setup;

internal static class LinuxBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		builder.RegisterType<X11ScreenCapture>().As<ScreenCapture>();
	}
}