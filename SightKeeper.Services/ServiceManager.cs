using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SightKeeper.DAL;
using SightKeeper.DAL.Members.Common;
using SightKeeper.DAL.Members.Detector;
using SightKeeper.DAL.Repositories;

namespace SightKeeper.Services;

public static class ServiceManager
{
	public static T GetService<T>() where T : notnull => Host.Services.GetRequiredService<T>();

	public static LoggingLevelSwitch LoggingLevelSwitch { get; } = new(); // TODO тут не место для этого, нужно выделить в отдельный сервис


	private static IHost BuildServices(string[]? args = null)
	{
		IHostBuilder? builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);
		builder.ConfigureServices(InitializeServices);
		return builder.Build();
	}
	
	
	private static IHost? _host;
	private static IHost Host => _host ??= BuildServices();
	
	
	private static void InitializeServices(this IServiceCollection services) =>
		services.AddLogger().AddDatabase().AddDbRepositories();

	private static IServiceCollection AddDatabase(this IServiceCollection services) => services.AddTransient<DbContext, AppDbContext>();

	private static IServiceCollection AddDbRepositories(this IServiceCollection services) => services
		.AddTransient<IRepository<DetectorModel>, EFRepository<DetectorModel>>()
		.AddTransient<IRepository<Game>, EFRepository<Game>>();

	private static IServiceCollection AddLogger(this IServiceCollection services) => services.AddSingleton(LoggerFactory);
	
	private static ILogger LoggerFactory(IServiceProvider arg)
	{
		LoggingLevelSwitch.MinimumLevel = LogEventLevel.Information;
		return new LoggerConfiguration()
			.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
			.WriteTo.Debug().MinimumLevel.ControlledBy(LoggingLevelSwitch).CreateLogger();
	}
}