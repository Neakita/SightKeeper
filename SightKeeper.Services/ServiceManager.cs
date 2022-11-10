using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SightKeeper.DAL;
using SightKeeper.Services.Configuration;

namespace SightKeeper.Services;

public static class ServiceManager
{
	static ServiceManager() => Builder = new ContainerBuilder();

	public static T Get<T>() where T : notnull => Container.Resolve<T>();

	public static LoggingLevelSwitch LoggingLevelSwitch { get; } = new(); // TODO тут не место для этого, нужно выделить в отдельный сервис
	
	public static ContainerBuilder Builder { get; }


	private static IContainer BuildServices()
	{
		Builder.Configure();
		return Builder.Build();
	}
	
	
	private static IContainer? _container;
	private static IContainer Container => _container ??= BuildServices();
}