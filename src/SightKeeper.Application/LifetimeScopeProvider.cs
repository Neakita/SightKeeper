using Autofac;

namespace SightKeeper.Application;

public interface LifetimeScopeProvider
{
	ILifetimeScope BeginLifetimeScope(object obj, ILifetimeScope scope);
}