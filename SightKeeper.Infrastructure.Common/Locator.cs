using Autofac;

namespace SightKeeper.Infrastructure.Common;

public static class Locator
{
	public static IContainer Container => _container ?? throw new InvalidOperationException($"{nameof(Locator)} is not initialized");
	
	public static TService Resolve<TService>() where TService : notnull => Container.Resolve<TService>();

	public static TService Resolve<TService, TParam>(TParam param)
		where TService : notnull
		where TParam : notnull =>
		Container.Resolve<TService>(new PositionalParameter(0, param));

	public static void Setup(IContainer container) => _container = container;

	private static IContainer? _container;
}
