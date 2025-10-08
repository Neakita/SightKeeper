using Autofac;
using SharpHook.Reactive;

namespace SightKeeper.Application;

public static class ContainerApplicationServicesExtensions
{
	public static void UseHotKeys(this IContainer container)
	{
		var hook = container.Resolve<IReactiveGlobalHook>();
		hook.RunAsync();
	}
}