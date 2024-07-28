using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Threading;
using Serilog;
using SightKeeper.Avalonia.Setup;

namespace SightKeeper.Avalonia;

internal static class Program
{
	// Initialization code. Don't use any Avalonia, third-party APIs or any
	// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
	// yet and stuff might break.
	[STAThread]
	public static void Main(string[] args)
	{
		try
		{
			AppBootstrapper.Setup();
			TaskScheduler.UnobservedTaskException += OnTaskSchedulerUnobservedException;
			BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
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
		// TODO should it really use dispatcher?
		Dispatcher.UIThread.InvokeAsync(() => LogUnhandledExceptions(e.Exception, nameof(TaskScheduler)), DispatcherPriority.Normal);
	}

	// Avalonia configuration, don't remove; also used by visual designer.
	public static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.LogToTrace();

	private static void LogUnhandledExceptions(Exception exception, string source)
	{
		Log.Fatal(exception, "Unhandled exception occured from {Source}", source);
	}
}