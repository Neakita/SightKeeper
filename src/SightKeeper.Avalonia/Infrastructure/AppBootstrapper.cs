using Autofac;
using SightKeeper.Application.Infrastructure;
using SightKeeper.Data;

namespace SightKeeper.Avalonia.Infrastructure;

internal static class AppBootstrapper
{
	public static IContainer Setup()
	{
		var builder = new ContainerBuilder();
		builder.RegisterServices();
		var container = builder.Build();
		container.InitializeServices();
		return container;
	}

	private static void RegisterServices(this ContainerBuilder builder)
	{
		var persistenceOptions = new PersistenceOptions
		{
			ClassifierDataSetScopeConfiguration = ApplicationServicesExtensions.RegisterClassifierServices,
			DetectorDataSetScopeConfiguration = ApplicationServicesExtensions.RegisterDetectorServices,
			PoserDataSetScopeConfiguration = ApplicationServicesExtensions.RegisterPoserServices
		};
		builder.RegisterPersistence(persistenceOptions);
		builder.RegisterApplicationServices();
		builder.RegisterOSSpecificServices();
		builder.RegisterAvaloniaServices();
		builder.RegisterLogger();
		builder.RegisterPresentationServices();
		builder.RegisterCommands();
		builder.RegisterViewModels();
	}

	private static void InitializeServices(this IContainer container)
	{
		container.LoadData();
		container.UseAutoSaving();
		container.UseHotKeys();
	}
}