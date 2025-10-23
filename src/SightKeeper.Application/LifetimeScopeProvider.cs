using Autofac;

namespace SightKeeper.Application;

public interface LifetimeScopeProvider
{
	ILifetimeScope GetLifetimeScope(object obj);
}