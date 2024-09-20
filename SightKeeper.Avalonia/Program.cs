using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Threading;
using Serilog;
using SightKeeper.Application.Linux;
using SightKeeper.Domain.Model;
using SixLabors.ImageSharp;

namespace SightKeeper.Avalonia;

internal static class Program
{
	// Initialization code. Don't use any Avalonia, third-party APIs or any
	// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
	// yet and stuff might break.
	[STAThread]
	public static void Main(string[] args)
	{
		X11ScreenCapture screenCapture = new();
		using var initial = screenCapture.Capture(new Vector2<ushort>(640, 640), null);
		if (File.Exists("Test.png"))
			File.Delete("Test.png");
		Image.Load(initial).Save("Test.png");
		DateTime start = DateTime.UtcNow;
		const int samples = 1000;
		for (int i = 0; i < samples; i++)
		{
			if (i % 100 == 0 && i != 0)
				Console.WriteLine($"{i} Elapsed {(DateTime.UtcNow - start).TotalMilliseconds / i}ms per capture");
			using var stream = screenCapture.Capture(new Vector2<ushort>(640, 640), null);
		}
		var end = DateTime.UtcNow - start;
		Console.WriteLine($"Elapsed {end.TotalMilliseconds / samples}ms per capture");
		Console.ReadKey(true);
		return;
		SetupLogger();
		AppBuilder? appBuilder = null;
		try
		{
			TaskScheduler.UnobservedTaskException += OnTaskSchedulerUnobservedException;
			appBuilder = BuildAvaloniaApp();
			appBuilder.StartWithClassicDesktopLifetime(args);
		}
		catch (Exception exception)
		{
			LogUnhandledExceptions(exception, $"{nameof(Program)}.{nameof(Main)}");
			throw;
		}
		finally
		{
			if (appBuilder?.Instance is IDisposable disposable)
				disposable.Dispose();
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