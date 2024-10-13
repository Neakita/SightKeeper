#if OS_WINDOWS
using Autofac;
using SightKeeper.Application;
using SightKeeper.Application.Games;
using SightKeeper.Application.Windows;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Avalonia.Setup;

internal static class WindowsBootstrapper
{
    public static void Setup(ContainerBuilder builder)
    {
        builder.RegisterType<WindowsGameIconProvider>().As<GameIconProvider>();
        builder.RegisterType<WindowsFileExplorerGameExecutableDisplayer>().As<GameExecutableDisplayer>();
        builder.RegisterType<DX11ScreenCapture>().As<ScreenCapture<Bgra32>>();
    }
}
#endif