using Autofac;
using SightKeeper.Application;
using SightKeeper.Data;

namespace SightKeeper.Avalonia;

internal static class AppBootstrapper
{
	public static IContainer Setup()
	{
		var builder = new ContainerBuilder();
		builder.AddPersistence();
		builder.AddApplicationServices();
		builder.AddOSSpecificServices();
		builder.AddAvaloniaServices();
		builder.AddLogger();
		builder.AddPresentationServices();
		builder.AddCommands();
		builder.AddViewModels();
		var container = builder.Build();
		container.UseBinarySerialization();
		container.LoadData();
		container.UseAutoSaving();
		container.UseHotKeys();
		return container;
	}
}