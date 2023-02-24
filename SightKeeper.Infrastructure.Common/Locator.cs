using Autofac;

namespace SightKeeper.Infrastructure.Common;

public static class Locator
{
	public static T Resolve<T>() where T : notnull => Container.Resolve<T>();

	public static void Setup(IContainer container) => _container = container;

	private static IContainer Container => _container ?? throw new InvalidOperationException($"{nameof(Locator)} is not initialized");
	private static IContainer? _container;
}
