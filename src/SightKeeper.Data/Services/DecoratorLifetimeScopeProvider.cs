using Autofac;
using SightKeeper.Application;
using SightKeeper.Domain;

namespace SightKeeper.Data.Services;

internal sealed class DecoratorLifetimeScopeProvider : LifetimeScopeProvider
{
	public ILifetimeScope GetLifetimeScope(object obj)
	{
		return obj.GetFirst<LifetimeScopeProviderDecorator>().LifetimeScope;
	}
}