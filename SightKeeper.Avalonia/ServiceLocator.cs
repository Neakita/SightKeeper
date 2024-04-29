using Autofac;
using SightKeeper.Avalonia.Setup;

namespace SightKeeper.Avalonia;

internal static class ServiceLocator
{
	internal static IContainer Instance { get; } = AppBootstrapper.Setup();
}