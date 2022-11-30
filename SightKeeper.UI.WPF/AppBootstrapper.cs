using ReactiveUI;
using Serilog;
using Serilog.Core;
using SightKeeper.DAL;
using SightKeeper.UI.WPF.ViewModels.Elements;
using SightKeeper.UI.WPF.ViewModels.Windows;
using SightKeeper.UI.WPF.Views.Windows;
using Splat;
using Splat.Serilog;

namespace SightKeeper.UI.WPF;

internal static class AppBootstrapper
{
	internal static void Setup()
	{
		SetupLogging();
		SetupDatabase();
		SetupViews();
		SetupViewModels();
		SplatRegistrations.SetupIOC();
	}
	

	private static void SetupLogging()
	{
		LoggingLevelSwitch loggingLevelSwitch = new();
		SplatRegistrations.RegisterConstant(loggingLevelSwitch);
		Log.Logger = new LoggerConfiguration()
			.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
			.MinimumLevel.ControlledBy(loggingLevelSwitch)
			.CreateLogger();
		Locator.CurrentMutable.UseSerilogFullLogger();
	}
	
	private static void SetupDatabase() => SplatRegistrations.Register<IAppDbContext, AppDbContext>();
	
	private static void SetupViews() => SplatRegistrations.Register<MainWindow>();

	private static void SetupViewModels()
	{
		SplatRegistrations.Register<MainWindowVM>();
		SplatRegistrations.Register<HamburgerMenuVM>();
	}
}
