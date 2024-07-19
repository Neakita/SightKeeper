using Autofac;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace SightKeeper.Avalonia.Setup;

internal static class AppBootstrapper
{
	public static IContainer Setup()
	{
		ContainerBuilder builder = new();
		SetupLogger(builder);
		// DataBaseBootstrapper.Setup(builder);
		ViewModelsBootstrapper.Setup(builder);
		ServicesBootstrapper.Setup(builder);
		return builder.Build();
	}

	public static void OnRelease()
	{
		DataBaseBootstrapper.OnRelease(ServiceLocator.Instance);
	}

	private static void SetupLogger(ContainerBuilder builder)
	{
		LoggingLevelSwitch levelSwitch = new(LogEventLevel.Verbose);
		builder.RegisterInstance(levelSwitch);
		Log.Logger = new LoggerConfiguration()
			.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
			#if DEBUG
			.WriteTo.Debug()
			#endif
			.WriteTo.Seq("http://localhost:5341/", apiKey: "YKxpWEmlEG0TwTHJIYuX")
			.MinimumLevel.ControlledBy(levelSwitch)
			.CreateLogger();
	}
}
