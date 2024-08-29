#if OS_WINDOWS
using Autofac;
using SightKeeper.Application.Games;
using SightKeeper.Application.Windows;

namespace SightKeeper.Avalonia.Setup;

internal static class WindowsBootstrapper
{
    public static void Setup(ContainerBuilder builder)
    {
        builder.RegisterType<WindowsGameIconProvider>().As<GameIconProvider>();
        builder.RegisterType<WindowsFileExplorerGameExecutableDisplayer>().As<GameExecutableDisplayer>();
    }
}
#endif