using System;
using System.Threading.Tasks;
using System.Windows;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SightKeeper.Backend;
using SightKeeper.Backend.Data;
using SightKeeper.Backend.Data.Members;

namespace SightKeeper.UI.WPF;

public partial class App
{
    #region Static members

    public static LogEventLevel MinimumLoggerLevel
    {
        get => _levelSwitch.MinimumLevel;
        set
        {
            _levelSwitch.MinimumLevel = value;
            Log.Logger.Information("Minimum level is set to {CurrentMinimumLevel}", value.ToString());
        }
    }

    private static LoggingLevelSwitch _levelSwitch = null!;

    #endregion

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        SetupLogging();
        SetupExceptionHandling();
        SetupDatabase();
    }

    private static void SetupDatabase()
    {
        using AppDbContext dbContext = new();
        if (dbContext.Database.EnsureCreated()) Log.Logger.Information("Database just created");
        else Log.Logger.Verbose("Database already exists");
    }

    private static void SetupLogging()
    {
        _levelSwitch = new LoggingLevelSwitch();
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.Debug()
            .MinimumLevel.ControlledBy(_levelSwitch).CreateLogger();
        Log.Logger.Verbose("Logger initialized");
    }

    private void SetupExceptionHandling()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            Log.Logger.Fatal((Exception) args.ExceptionObject, "An unhandled exception occured");

        DispatcherUnhandledException += (_, args) =>
        {
            Log.Logger.Fatal(args.Exception, "An unhandled dispatcher exception occured");
            args.Handled = true;
        };

        TaskScheduler.UnobservedTaskException += (_, args) =>
        {
            Log.Logger.Fatal(args.Exception, "An unhandled unobserved task exception occured");
            args.SetObserved();
        };
    }
}