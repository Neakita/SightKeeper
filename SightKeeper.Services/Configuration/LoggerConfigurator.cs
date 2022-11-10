using Autofac;
using Serilog;
using Serilog.Core;

namespace SightKeeper.Services.Configuration;

public static class LoggerConfigurator
{
	public static ContainerBuilder ConfigureLogger(this ContainerBuilder builder)
	{
		builder.RegisterInstance(Logger).As<ILogger>();
		builder.RegisterInstance(LevelSwitch).As<LoggingLevelSwitch>();
		return builder;
	}


	private static Logger Logger => new LoggerConfiguration()
			.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
			.WriteTo.Debug().MinimumLevel.ControlledBy(LevelSwitch).CreateLogger();

	private static readonly LoggingLevelSwitch LevelSwitch = new();
}
