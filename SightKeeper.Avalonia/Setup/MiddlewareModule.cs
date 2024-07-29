using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Core.Resolving.Pipeline;

namespace SightKeeper.Avalonia.Setup;

internal sealed class MiddlewareModule : Autofac.Module
{
	public MiddlewareModule(IResolveMiddleware middleware)
	{
		_middleware = middleware;
	}

	protected override void AttachToComponentRegistration(
		IComponentRegistryBuilder componentRegistryBuilder,
		IComponentRegistration registration)
	{
		registration.PipelineBuilding += (_, pipeline) =>
		{
			pipeline.Use(_middleware);
		};
	}

	private readonly IResolveMiddleware _middleware;
}