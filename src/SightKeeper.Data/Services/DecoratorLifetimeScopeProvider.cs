using Autofac;
using SightKeeper.Application;
using SightKeeper.Domain;

namespace SightKeeper.Data.Services;

internal sealed class DecoratorLifetimeScopeProvider : LifetimeScopeProvider
{
	public ILifetimeScope BeginLifetimeScope(object obj, ILifetimeScope scope)
	{
		return obj.GetFirst<LifetimeScopeProviderDecorator>().BeginLifetimeScope(scope);
	}
}