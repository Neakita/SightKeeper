using Autofac;
using SharpHook.Reactive;

namespace SightKeeper.Application.Infrastructure;

public static class ApplicationBootstrapExtensions
{
	public static void UseHotKeys(this IContainer container)
	{
		var hook = container.Resolve<IReactiveGlobalHook>();
		hook.RunAsync();
	}
}