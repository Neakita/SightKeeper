using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SightKeeper.Services.Registrators;

namespace SightKeeper.Services;

public static class ServiceManager
{
	public static T GetService<T>() where T : notnull => Host.Services.GetRequiredService<T>();


	private static IHost BuildServices(string[]? args = null)
	{
		IHostBuilder? builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);
		builder.ConfigureServices(InitializeServices);
		return builder.Build();
	}
	
	
	private static IHost? _host;
	private static IHost Host => _host ??= BuildServices();
	
	
	private static void InitializeServices(this IServiceCollection services) =>
		services.AddDatabase().AddDbRepositories();
}