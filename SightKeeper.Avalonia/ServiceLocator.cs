using Autofac;

namespace SightKeeper.Avalonia;

public static class ServiceLocator
{
    public static IContainer Instance => _instance ??= AppBootstrapper.Setup();
    private static IContainer? _instance;
}