using Autofac;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia;

internal static class ServiceLocator
{
    internal static IContainer Instance
    {
        get
        {
            Guard.IsNotNull(_instance);
            return _instance;
        }
    }

    internal static void Setup(IContainer container)
    {
        if (_instance != null)
            ThrowHelper.ThrowInvalidOperationException($"{nameof(ServiceLocator)} has already been setup");
        _instance = container;
    }
    
    private static IContainer? _instance;
}