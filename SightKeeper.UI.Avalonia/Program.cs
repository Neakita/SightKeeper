using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Threading;
using Material.Icons;
using ReactiveUI;
using Serilog;
using SightKeeper.UI.Avalonia.Views.Windows;

namespace SightKeeper.UI.Avalonia;

class Program
{
	// Initialization code. Don't use any Avalonia, third-party APIs or any
	// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
	// yet and stuff might break.
	[STAThread]
	public static void Main(string[] args)
	{
		try
		{
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
			RxApp.DefaultExceptionHandler = Observer.Create<Exception>(exception => HandleUnhandledExceptions(exception, "RxApp.DefaultExceptionHandler"));
			BuildAvaloniaApp()
				.StartWithClassicDesktopLifetime(args);
		}
		catch (Exception exception)
		{
			HandleUnhandledExceptions(exception, "UI thread");
		}
	}

	private static void TaskSchedulerOnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
	{
		Dispatcher.UIThread.InvokeAsync(() => HandleUnhandledExceptions(e.Exception, "TaskScheduler"), DispatcherPriority.Normal);
	}

	// Avalonia configuration, don't remove; also used by visual designer.
	public static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.LogToTrace()
			.UseReactiveUI();

	private static void HandleUnhandledExceptions(Exception exception, string source)
	{
		Log.Fatal(exception, "Unhandled exception occured from {Source}", source);
		MessageBoxDialog.Show(exception.Message, MessageBoxDialog.DialogResult.Ok, "Unhandled exception", MaterialIconKind.Alert);
	}
}