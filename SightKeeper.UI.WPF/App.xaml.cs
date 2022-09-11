using System;
using System.Threading.Tasks;
using System.Windows;
using Serilog;
using SightKeeper.Backend.Data;

namespace SightKeeper.UI.WPF;

public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        SetupExceptionHandling();
        SetupDatabase();
    }

    private static void SetupDatabase()
    {
        using AppDbContext dbContext = new();
        dbContext.Database.EnsureCreated();
    }

    private void SetupExceptionHandling()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Debug().CreateLogger();

        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            LogUnhandledException((Exception) args.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

        DispatcherUnhandledException += (_, args) =>
        {
            LogUnhandledException(args.Exception, "Application.Current.DispatcherUnhandledException");
            args.Handled = true;
        };

        TaskScheduler.UnobservedTaskException += (_, args) =>
        {
            LogUnhandledException(args.Exception, "TaskScheduler.UnobservedTaskException");
            args.SetObserved();
        };
    }

    private static void LogUnhandledException(Exception exception, string source)
    {
        string message = $"Unhandled exception ({source})";
        try
        {
            System.Reflection.AssemblyName assemblyName =
                System.Reflection.Assembly.GetExecutingAssembly().GetName();
            message = $"Unhandled exception in {assemblyName.Name} v{assemblyName.Version}";
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Exception in LogUnhandledException");
        }
        finally
        {
            Log.Logger.Error(exception, message);
        }
    }
}