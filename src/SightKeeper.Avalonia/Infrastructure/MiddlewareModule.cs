using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Core.Resolving.Pipeline;

namespace SightKeeper.Avalonia.Infrastructure;

internal sealed class MiddlewareModule(IResolveMiddleware middleware) : Module
{
	protected override void AttachToComponentRegistration(
		IComponentRegistryBuilder componentRegistryBuilder,
		IComponentRegistration registration)
	{
		registration.PipelineBuilding += (_, pipeline) =>
		{
			pipeline.Use(middleware);
		};
	}
}