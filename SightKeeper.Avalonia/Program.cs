using System;
using System.Threading.Tasks;
using Avalonia;
using Serilog;
using Serilog.Events;

namespace SightKeeper.Avalonia;

internal static class Program
{
	// Initialization code. Don't use any Avalonia, third-party APIs or any
	// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
	// yet and stuff might break.
	[STAThread]
	public static void Main(string[] args)
	{
		SetupLogger();
		try
		{
			TaskScheduler.UnobservedTaskException += OnTaskSchedulerUnobservedException;
			var appBuilder = BuildAvaloniaApp();
			appBuilder.StartWithClassicDesktopLifetime(args);
		}
		catch (Exception exception)
		{
			LogUnhandledExceptions(LogEventLevel.Fatal, exception, $"{nameof(Program)}.{nameof(Main)}");
		}
		finally
		{
			Log.CloseAndFlush();
		}
	}

	private static void OnTaskSchedulerUnobservedException(object? sender, UnobservedTaskExceptionEventArgs exception)
	{
		LogUnhandledExceptions(LogEventLevel.Error, exception.Exception, nameof(TaskScheduler));
	}

	// Avalonia configuration, don't remove; also used by visual designer.
	private static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.LogToTrace();

	private static void LogUnhandledExceptions(LogEventLevel level, Exception exception, string source)
	{
		Log.Write(level, exception, "Unhandled exception occured from {Source}", source);
	}

	private static void SetupLogger()
	{
		Log.Logger = new LoggerConfiguration()
			.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
			.WriteTo.Debug()
			.CreateLogger();
	}
}