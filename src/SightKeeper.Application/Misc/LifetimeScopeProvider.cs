using Autofac;

namespace SightKeeper.Application.Misc;

public interface LifetimeScopeProvider
{
	ILifetimeScope BeginLifetimeScope(object obj, ILifetimeScope scope);
}