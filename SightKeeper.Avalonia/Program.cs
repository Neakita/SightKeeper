using System;
using System.Threading.Tasks;
using Avalonia;
using Serilog;

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
			LogUnhandledExceptions(exception, $"{nameof(Program)}.{nameof(Main)}");
			throw;
		}
		finally
		{
			Log.CloseAndFlush();
		}
	}

	private static void OnTaskSchedulerUnobservedException(object? sender, UnobservedTaskExceptionEventArgs e)
	{
		LogUnhandledExceptions(e.Exception, nameof(TaskScheduler));
	}

	// Avalonia configuration, don't remove; also used by visual designer.
	private static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.LogToTrace();

	private static void LogUnhandledExceptions(Exception exception, string source)
	{
		Log.Fatal(exception, "Unhandled exception occured from {Source}", source);
	}

	private static void SetupLogger()
	{
		Log.Logger = new LoggerConfiguration()
			.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
#if DEBUG
			.WriteTo.Debug()
#endif
			.WriteTo.Seq("http://localhost:5341/", apiKey: "YKxpWEmlEG0TwTHJIYuX")
			.CreateLogger();
	}
}