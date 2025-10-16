using Autofac;
using SightKeeper.Application;
using SightKeeper.Data;

namespace SightKeeper.Avalonia.Infrastructure;

internal static class AppBootstrapper
{
	public static IContainer Setup()
	{
		var builder = new ContainerBuilder();
		builder.AddServices();
		var container = builder.Build();
		container.InitializeServices();
		return container;
	}

	private static void AddServices(this ContainerBuilder builder)
	{
		builder.AddPersistence();
		builder.AddApplicationServices();
		builder.AddOSSpecificServices();
		builder.AddAvaloniaServices();
		builder.AddLogger();
		builder.AddPresentationServices();
		builder.AddCommands();
		builder.AddViewModels();
	}

	private static void InitializeServices(this IContainer container)
	{
		container.LoadData();
		container.UseAutoSaving();
		container.UseHotKeys();
	}
}