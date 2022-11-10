using Autofac;

namespace SightKeeper.Services.Configuration;

public static class Configurator
{
	public static void Configure(this ContainerBuilder builder) => builder.ConfigureLogger();
}
