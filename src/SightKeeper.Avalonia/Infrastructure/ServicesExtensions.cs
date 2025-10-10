using Autofac;
using SightKeeper.Application;
using SightKeeper.Avalonia.Misc;
#if OS_WINDOWS
using SightKeeper.Application.Windows;
#elif OS_LINUX
using SightKeeper.Application.Linux;
#endif

namespace SightKeeper.Avalonia.Infrastructure;

internal static class ServicesExtensions
{
	public static void AddAvaloniaServices(this ContainerBuilder builder)
	{
		builder.RegisterType<AvaloniaSelfActivityProvider>()
			.As<SelfActivityProvider>();
	}

	public static void AddOSSpecificServices(this ContainerBuilder builder)
	{
#if OS_WINDOWS
		builder.AddWindowsServices();
#elif OS_LINUX
		builder.AddLinuxServices();
#endif
	}

	public static void AddLogger(this ContainerBuilder builder)
	{
		builder.RegisterModule(new MiddlewareModule(new SerilogMiddleware()));
	}
}