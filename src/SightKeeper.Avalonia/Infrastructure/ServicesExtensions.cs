using System.Collections.Generic;
using Autofac;
using Serilog;
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
	public static void RegisterAvaloniaServices(this ContainerBuilder builder)
	{
		builder.RegisterType<AvaloniaSelfActivityProvider>()
			.As<SelfActivityProvider>();
	}

	public static void RegisterOSSpecificServices(this ContainerBuilder builder)
	{
#if OS_WINDOWS
		builder.RegisterWindowsServices();
#elif OS_LINUX
		builder.RegisterLinuxServices();
#endif
	}

	public static void RegisterLogger(this ContainerBuilder builder)
	{
		builder.Register(_ => Log.Logger);
		builder.RegisterComposite<ILogger>((_, services) => CreateCompositeLogger(services));
		builder.RegisterModule(new MiddlewareModule(new SerilogMiddleware()));
	}

	private static ILogger CreateCompositeLogger(IEnumerable<ILogger> loggers)
	{
		var loggerConfiguration = new LoggerConfiguration()
			.MinimumLevel.Verbose();
		foreach (var logger in loggers)
			loggerConfiguration.WriteTo.Logger(logger);
		return loggerConfiguration.CreateLogger();
	}
}