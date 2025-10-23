using Autofac;

namespace SightKeeper.Data.Services;

internal interface LifetimeScopeProviderDecorator
{
	ILifetimeScope LifetimeScope { get; }
}