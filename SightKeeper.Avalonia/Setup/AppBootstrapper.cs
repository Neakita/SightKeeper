using Autofac;

namespace SightKeeper.Avalonia.Setup;

internal static class AppBootstrapper
{
	public static IContainer Setup()
	{
		ContainerBuilder builder = new();
		builder.RegisterModule(new MiddlewareModule(new SerilogMiddleware()));
		ViewModelsBootstrapper.Setup(builder);
		ServicesBootstrapper.Setup(builder);
		return builder.Build();
	}
}
